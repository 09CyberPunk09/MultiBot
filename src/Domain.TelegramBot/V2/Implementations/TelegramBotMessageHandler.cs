using Application.Telegram.Implementations;
using Application.TextCommunication.Core.Caching;
using Application.TextCommunication.Core.Interfaces;
using Application.TextCommunication.Core.PipelineBaseKit;
using Application.TextCommunication.Core.Routing;
using Common.Dto;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Persistence.Caching.Redis;
using ServiceStack;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using MessageContext = Application.TextCommunication.Core.Context.MessageContext;

namespace Application.TelegramBot.Pipelines.V2.Implementations;

public class TelegramBotMessageHandler : IMessageHandler
{
    private readonly IHost _host;
    private readonly IMessageSender _sender;
    private readonly Cache _cache;
    private readonly RoutingTable _routingTable;
    private readonly MessageReceiver _receiver; 

    public TelegramBotMessageHandler(
        IHost host, 
        IMessageSender sender, 
        RoutingTable routingTable,
        ITelegramBotClient client)
    {
        _host = host;
        _sender = sender;
        _routingTable = routingTable;
        _receiver = new(this);
        _cache = new(DatabaseType.Pipelines);
    }

    public async Task HandleMessage(TelegramMessage message)
    {
        try
        {
            bool currentStageIsExternal = false;

            #region find chat data in cache
            var chatId = message.ChatId;
            var cachedChatData = _cache.Get<CachedChatData>(chatId.ToString());
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
                command = _routingTable.GetCommand(commandPayload.Command);
                if(commandPayload.Stage != null)//if a stage that should be executed is an external form the command - we find gis type
                {
                    stageType = _routingTable.GetStageType(commandPayload.Stage);
                    currentStageIsExternal = true;
                }
                else if(commandPayload.StageIndex.HasValue)//if not - we go with a standard way by finding the stage by index in the command seq
                {
                    stageType =  command.StagesSequence[commandPayload.StageIndex.Value];
                }
            }
            else//2.2 If the command isnt null - it means that the message IS a command call
            {
                stageType = command.Type;//and we set the command type as stage type, because the first stage of a command is the command itself
            }
            #endregion

            #region build context
            var context = new MessageContext()
            {
                Message = new()
                {
                    Files = message.Files,
                    Text = message.Text,
                },
                User = chatDataFacade.Get<ShortUserInfoDto>(),
                Cache = chatDataFacade,
                PipelineContext = new()
                {
                    CommandMetadata = command
                }
            };
            #endregion

            #region execute the stage
            if (stageType == null)
            {
                await _sender.SendMessageAsync(new AdressedContentResult()
                {
                    Text = "Sorry, I could not find any responding pipeline",
                    ChatId = message.ChatId,
                });
                return;
            }

            var stage = _host.Container.GetService(stageType) as IStage;
            var result = await stage.Execute(context);
            var contentResult = result.Content;
            _sender.SendMessage(new AdressedContentResult()
            {
                ChatId = message.ChatId,

                MultiMessages = contentResult.MultiMessages,
                Edited = contentResult.Edited,
                IsEmpty = contentResult.IsEmpty,
                Menu = contentResult.Menu,
                Photo = contentResult.Photo,
                Text = contentResult.Text,
            });
            #endregion

            #region set next pipeline stage and command
            StageDto nextStageData = new();
            var currentStageData = chatDataFacade.Get<StageDto>();
            if (result.NextStage != null)//if a stage has defined his own next stage
            {
                nextStageData.Stage = result.NextStage;//we set it as the next stage
                nextStageData.Command = command.Route.Route;//and save the current route under which we execute the next stage

               if(currentStageData.StageIndex != -1)
               {
                    int nextStageIndex = GetNextStageIndex(command.StagesSequence, stageType.FullName);//then, we get the next stage index for knowledge when the command map iteration should continue
                    nextStageData.StageIndex = nextStageIndex;//and set it for save
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

            _cache.Set(chatId.ToString(), chatDataFacade.Data);//save the data
            #endregion
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public Task StartReceiving()
    {
        _receiver.StartReceiving();
        return Task.CompletedTask;
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
    private int GetNextStageIndex(StageSequence seq,int currentPosition)
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
class StageDto
{
    /// <summary>
    /// Command text
    /// </summary>
    public string Command { get; set; }
    /// <summary>
    /// Type name of a stage.Is used only if a stage is not in the command stage map
    /// </summary>
    public string Stage { get; set; }
    /// <summary>
    /// is used to iterate through the command stage map
    /// </summary>
    public int? StageIndex { get; set; }
}