using Microsoft.Extensions.DependencyInjection;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Caching;
using TelegramBot.ChatEngine.Commands.Interfaces;
using TelegramBot.ChatEngine.Commands.Middlewares;
using TelegramBot.ChatEngine.Commands.PipelineBaseKit;
using TelegramBot.ChatEngine.Commands.Repsonses;
using TelegramBot.ChatEngine.Commands.Routing;
using TelegramBot.ChatEngine.Core;
using TelegramBot.ChatEngine.Implementation.Dro;
using TelegramBot.ChatEngine.Setup;

namespace TelegramBot.ChatEngine.Implementation;

public class TelegramBotMessageHandler : IMessageHandler
{
    private readonly Func<ContentResultV2, Task<SentTelegramMessage>> _sender;
    private readonly IServiceProvider _serviceProvider;
    private readonly IBotCache _cache;
    private readonly RoutingTable _routingTable;
    private readonly MiddlewareHandler _perCommandMiddlewareHandler;
    private readonly MiddlewareHandler _perMessageMiddlewareHandler;
    //TODO: ADDMESSAGE DEFAULTS AS ERROR TEXT
    private readonly MessagingDefaults _messageDefaults;
    //NOW: GET LOGGER FROM LOGGING BUILDER


    public TelegramBotMessageHandler(
        IServiceProvider serviceProvider,
        Func<ContentResultV2, Task<SentTelegramMessage>> senderAction,
        MiddlewareHandler perCommandMiddleware,
        MiddlewareHandler perMessageMiddleware,
        MessagingDefaults messagingDefaults)
    {
        _perCommandMiddlewareHandler = perCommandMiddleware;
        _perMessageMiddlewareHandler = perMessageMiddleware;
        _sender = senderAction;
        _serviceProvider = serviceProvider;
#pragma warning disable CS8601 // Possible null reference assignment.
        _cache = serviceProvider.GetService<IBotCache>();
        _routingTable = serviceProvider.GetService<RoutingTable>();
#pragma warning restore CS8601 // Possible null reference assignment.
    }

    public async Task HandleMessage(TelegramMessage message)
    {
        //logger.Info($"Started handling message {message.Text}");
        try
        {
            bool currentStageIsExternal = false;
            bool isCommand = false;
            #region find chat data in cache
            var chatId = message.ChatId;

            var cachedChatData = new CachedChatData
            {
                Data = _cache.GetDictionary(chatId.ToString())
            };
            var chatDataFacade = new CachedChatDataWrapper(cachedChatData);
            #endregion

            #region determine stage
            //1.First, we try to identify the text written by user as command call
            Type stageType = null;
            var command = _routingTable.GetCommand(message.Text);
            if (command == null)
            {
                //2.If we didnt find any matches - we take the existing user cache data
                var commandPayload = chatDataFacade.Get<StageDto>();

                if (commandPayload == null)// if commandPayload is nill, that means thatthe cache does not contain any info about the command and the text is not recognized as command as well
                {
                    await _sender(new ContentResultV2()
                    {
                        Text = "Could not recognize text as command",
                        ChatId = chatId
                    });
                    return;
                }
                command = _routingTable.GetCommand(commandPayload.Command);

                if (commandPayload.Stage != null)//if a stage that should be executed is an external form the command - we find gis type
                {
                    stageType = _routingTable.GetStageType(commandPayload.Stage);
                    currentStageIsExternal = true;
                }
                else if (commandPayload.StageIndex.HasValue)//if not - we go with a standard way by finding the stage by index in the command seq
                {
                    stageType = command.StagesSequence[commandPayload.StageIndex.Value];
                }
            }
            else//2.2 If the command isnt null - it means that the message IS a command call
            {
                isCommand = true;
                stageType = command.Type;//and we set the command type as stage type, because the first stage of a command is the command itself
            }
            #endregion

            #region build context
            var context = new TelegramMessageContext()
            {
                RecipientChatId = chatId,
                RecipientUserId = chatId,//TODO:CHANFE
                Message = new()
                {
                    Files = message.Files,
                    Text = message.Text,
                },
                Cache = chatDataFacade,
                PipelineContext = new()
                {
                    CommandMetadata = command
                }
            };
            #endregion

            #region 
            if (stageType == null)
            {
                ////NOW: ADD IN BUILDER DEFAULT ERROR MESSAGE
                await _sender(new ContentResultV2()
                {
                    Text = "Could not recognize text as command",
                    ChatId = chatId
                });
            }

            #region Execute middlewares before stage
            if (isCommand)
            {
                var moveForward = await _perCommandMiddlewareHandler.ExecuteMiddlewares(context);
                if (!moveForward)
                    return;
            }
            var middlewareExecutedSuccessfully = await _perMessageMiddlewareHandler.ExecuteMiddlewares(context);
            if (!middlewareExecutedSuccessfully)
                return;

            #endregion execute stage and send a message
            StageResult result;
            if (isCommand)//if a stage that should be executed is a command entered from a user
            {
                var stage = _serviceProvider.GetService(stageType) as ITelegramCommand;
                result = await stage.Execute(context);
            }
            else//if a stage is a stage
            {
                var stage = _serviceProvider.GetService(stageType) as ITelegramStage;//we try to resolve it
                if (stage != null)//if everything is OK we exec the stage
                {
                    result = await stage.Execute(context);
                }
                else//if not - it means that 
                {
                    var commandStage = _serviceProvider.GetService(stageType) as ITelegramCommand;// the stage can be located in two ways: in the standard way,and when it is a command which is used as a stage
                    if (commandStage == null)//it means that the stage could not found and there is an error
                    {
                        throw new NullReferenceException();
                    }
                    result = await commandStage.Execute(context);
                }

            }
            var contentResult = result.Content;
            if (contentResult != null)
            {
                var response = await _sender(new ContentResultV2()
                {
                    ChatId = message.ChatId,
                    LastBotMessageId = chatDataFacade.Get<int?>(nameof(ContentResultV2.LastBotMessageId)),
                    MultiMessages = contentResult.MultiMessages,
                    Edited = contentResult.Edited,
                    Menu = contentResult.Menu,
                    Photo = contentResult.Photo,
                    Text = contentResult.Text,
                });

                //TODO: Move to messagesender
                chatDataFacade.Set(nameof(ContentResultV2.LastBotMessageId), response.SentMessage.MessageId);
            }
            #endregion

            #region set next pipeline stage and command
            StageDto nextStageData = new();
            var currentStageData = chatDataFacade.Get<StageDto>();
            //if the stage didnt forbid the next stage invokation
            if (context.Response.CanIvokeNext)
            {
                //we determine the next stage
                if (result.NextStage != null)//if a stage has defined his own next stage
                {
                    nextStageData.Stage = result.NextStage;//we set it as the next stage
                    nextStageData.Command = command.Route.Route;//and save the current route under which we execute the next stage

                    if (currentStageData.StageIndex != -1 && !currentStageIsExternal)
                    {
                        int nextStageIndex = GetNextStageIndex(command.StagesSequence, stageType.FullName);//then, we get the next stage index for knowledge when the command map iteration should continue
                        nextStageData.StageIndex = nextStageIndex;//and set it for save
                    }
                    else
                    {
                        nextStageData.StageIndex = currentStageData.StageIndex;
                    }
                }
                else//if a stage has not defined stages by itself we do the standard stuff
                {
                    int nextStageIndex = -1;
                    if (currentStageIsExternal)//if we came from the stage which is external to the stage map we dont change the next stage index, because it is initialized as a entrypoint-back to the command
                    {
                        nextStageIndex = currentStageData.StageIndex.Value;
                    }
                    else//if not - we do the standard stuff
                    {
                        nextStageIndex = GetNextStageIndex(command.StagesSequence, stageType.FullName);
                    }

                    if (nextStageIndex != -1)//if there are stages after current - we save the data about it in the cache
                    {
                        nextStageData.StageIndex = nextStageIndex;
                        nextStageData.Command = command.Route.Route;
                    }
                    else//if not - we clear the stage data, that means that there is no next stage and command
                    {
                        nextStageData.Stage = null;
                        nextStageData.Command = null;
                    }
                }

                chatDataFacade.Set(nextStageData);//set the next stage data built in previous steps
            }
            else
            {
                chatDataFacade.Set(currentStageData);
            }

            _cache.SetDictionary(chatId.ToString(), chatDataFacade.Data.Data);//save the data

            if (context.Response.InvokeNextImmediately)
            {
                //if we invoke immediately the next stage - his payload sould be null, so we empty the message
                message.Text = string.Empty;
                await HandleMessage(message);
            }
            #endregion
        }
        catch (Exception ex)
        {
            //logger.Error(ex);
        }
    }


    #region private methods

    private int GetNextStageIndex(StageSequence seq, string currentStageFullName)
    {
        var stageSequence = seq.Types;//pseudonim

        int currentStageIndex = Array.FindIndex(stageSequence.ToArray(), x => x.FullName == currentStageFullName);//first, we find the position of a current type in the sequence
        return GetNextStageIndex(seq, currentStageIndex);//then, we use it as a parameter of current position
    }

    /// <summary>
    /// Calculates the index of the next item in addition of the current position
    /// </summary>
    /// <param name="seq">The sequence where we search</param>
    /// <param name="currentPosition">The position in addition of which we do the search</param>
    /// <returns>-1 if the index is out of the array, or another value if everything is OK</returns>
    private int GetNextStageIndex(StageSequence seq, int currentPosition)
    {
        var stageSequence = seq.Types;//pseudonim

        int nextStageIndex = -1;
        if (currentPosition < stageSequence.Count - 1)
        {
            nextStageIndex = currentPosition + 1;
        }
        else
        {
            nextStageIndex = -1;
        }
        return nextStageIndex;
    }
    #endregion
}
