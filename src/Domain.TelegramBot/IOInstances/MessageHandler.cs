using Application.Services;
using Application.TelegramBot.Pipelines.IOInstances.Interfaces;
using Autofac;
using Domain.TelegramBot.IOInstances;
using Domain.TelegramBot.MessagePipelines.Start;
using Infrastructure.TelegramBot.ResponseTypes;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using NLog;
using Persistence.Caching.Redis.TelegramCaching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TextUI.Core.PipelineBaseKit;

namespace Infrastructure.TelegramBot.IOInstances
{
    public class MessageHandler
    {
        #region Cache Keys

        public const string CURRENT_MESSAGEPIPELINE_STAGE_INDEX = "CurrntPipelineStageIndex";
        public const string CURRENT_MESSAGEPIPELINE_TYPE_NAME = "CurrentMessagePipelineCommand";

        #endregion Cache Keys

        #region Injected members

        private readonly IMessageSender _sender;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly TelegramCache _cache;
        private readonly UserAppService _userService;
        private MessageReceiver _receiver;
        private readonly MiddlewareHandler _middlewareHandler;

        #endregion Injected members

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly Dictionary<RouteAttribute, Type> pipleineCommands;

        public MessageHandler(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
            _sender = lifetimeScope.Resolve<IMessageSender>();
            _cache = new();
            _userService = lifetimeScope.Resolve<UserAppService>();
            _middlewareHandler = lifetimeScope.Resolve<MiddlewareHandler>();
        }

        public void StartReceiving()
        {
            _receiver = new(this);
            _receiver.StartReceiving();
        }

        static MessageHandler()
        {
            pipleineCommands = GetPipelineTypes().ToDictionary(x => x.GetCustomAttribute<RouteAttribute>());
        }

        public void ConsumeMessage(Message message)
        {
            bool firstPerPipeline = true;
            MessagePipelineBase pipeline = MatchPipeline(message.Text);
            if(pipeline == null)
            {
                firstPerPipeline = false;
                var alreadySetPipleineName = _cache.GetValueForChat<string>(CURRENT_MESSAGEPIPELINE_TYPE_NAME, message.ChatId);
                if (alreadySetPipleineName != null)
                {
                    pipeline = GetExisting(alreadySetPipleineName);
                }
            }
            ExecutePieplineStage(pipeline, message, firstPerPipeline);
        }

        public void AddMiddleware<TMiddleware>() where TMiddleware : IMessageHandlerMiddleware
        {
            logger.Info($"Registered middleware {typeof(TMiddleware).Name}");
            _middlewareHandler.AddMiddleware<TMiddleware>();
        }


        private void ExecutePieplineStage(MessagePipelineBase pipeline, Message message, bool firstTimePerCommand = false)
        {
            if (pipeline != null)
            {
                MessageContext ctx = new(message.ChatId)
                {
                    Message = message,
                    TimeStamp = DateTime.Now,
                    User = _userService.GetByTgId(message.UserId),
                    RecipientUserId = message.UserId,
                    CurrentPipeline = new()
                    {
                        Instance = pipeline,
                        Type = pipeline.GetType()
                    }
                };
                var middlewaresPassed = _middlewareHandler.ExecuteMiddlewares(ctx).Result;
                if (!middlewaresPassed)
                {
                    return;
                }
                int stageIndex;
                if (!firstTimePerCommand)
                {
                    stageIndex = _cache.GetValueForChat<int>(CURRENT_MESSAGEPIPELINE_STAGE_INDEX, ctx.RecipientChatId);
                }
                else
                {
                    stageIndex = -1;
                }
                pipeline.MessageContext = ctx;
                pipeline.Response.CanIvokeNext = true;
                var result = pipeline.Execute(ctx, pipeline.GetType(), stageIndex);

                _sender.SendMessage(result.Result);

                if (result.PipelineEnded)
                {
                    //remove value from cache
                    _cache.GetValueForChat<string>(CURRENT_MESSAGEPIPELINE_STAGE_INDEX, ctx.RecipientChatId, true);
                    _cache.GetValueForChat<string>(CURRENT_MESSAGEPIPELINE_TYPE_NAME, ctx.RecipientChatId, true);
                    return;
                }
                if (result.CanIvokeNext)
                {
                    _cache.SetValueForChat(CURRENT_MESSAGEPIPELINE_TYPE_NAME, result.NextPipelineTypeFullName, ctx.RecipientChatId);
                    _cache.SetValueForChat(CURRENT_MESSAGEPIPELINE_STAGE_INDEX, result.NextStageIndex, ctx.RecipientChatId);

                }
                else
                {
                    _cache.SetValueForChat(CURRENT_MESSAGEPIPELINE_TYPE_NAME, pipeline.GetType().FullName, ctx.RecipientChatId);
                    _cache.SetValueForChat(CURRENT_MESSAGEPIPELINE_STAGE_INDEX, stageIndex, ctx.RecipientChatId);
                }
            }
            else
            {
                _sender.SendMessage(new TextResult("No corresponding pipeline found", message.ChatId));
            }
        }
        private MessagePipelineBase MatchPipeline(string text)
        {
            Type matchingPipeline;
            var matchingPipelines = pipleineCommands.Where(x =>
            {
                return x.Key.Route == text || x.Key.AlternativeRoute == text;
            });
            if (matchingPipelines.Any())
            {
                matchingPipeline = matchingPipelines.FirstOrDefault().Value;
                return _lifetimeScope.Resolve(matchingPipeline) as MessagePipelineBase;
            }
            else
            {
                return null;
            }
        }

        private MessagePipelineBase GetExisting(string fullName)
        {
            var type = typeof(StartPipeline).Assembly.GetTypes().FirstOrDefault(x => x.FullName == fullName);
            if(type == null)
            {
                return null;
            }
            return _lifetimeScope.Resolve(type) as MessagePipelineBase;
        }

        private static List<Type> GetPipelineTypes()
        {
            var basePipelineType = typeof(MessagePipelineBase);
            return typeof(StartPipeline).Assembly.GetTypes().Where(t => t.IsSubclassOf(basePipelineType)).ToList();
        }
    }
}
