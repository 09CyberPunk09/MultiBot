using Autofac;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.Core.Types;
using Infrastructure.UI.TelegramBot.MessagePipelines;
using Infrastructure.UI.TelegramBot.ResponseTypes;
using Persistence.Caching.Redis.TelegramCaching;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;

namespace Infrastructure.UI.TelegramBot.IOInstances
{
    public class MessageConsumer
    {
        #region Cache Keys
        private const string CURRENT_MESSAGEPIPELINE_STAGE_NAME = "CurrntPipelineStageName";
        private const string CURRENT_MESSAGEPIPELINE_COMMAND = "CurrentMessagePipelineCommand";
        #endregion

        #region Injected members
        private readonly ITelegramBotClient _uiClient;
        private readonly IResultSender _sender;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly TelegramCache _cache;
        #endregion

        private static readonly Dictionary<string, Type> pipleineCommands;
        public IMessagePipeline DefaultPipeline { get; set; }

        public MessageConsumer(IResultSender sender,
                               ILifetimeScope lifetimeScope,
                               ITelegramBotClient uiClient)
        {
            _sender = sender;
            _lifetimeScope = lifetimeScope;
            _uiClient = uiClient;

            _cache = new();
        }

        static MessageConsumer()
        {
            //TODO:  make attribute multiparametrized,duplicate in list every entry which has more than one command
            pipleineCommands = GetPipelineTypes().ToDictionary(x => (x.GetCustomAttributes(true).FirstOrDefault(attr => (attr as RouteAttribute) != null) as RouteAttribute).Route);
        }

        public void ConsumeMessage(Core.Types.Message message)
        {
            MessageContext ctx = new()
            {
                Message = message,
                MoveNext = true,
                Recipient = message.ChatId,
                TimeStamp = DateTime.Now
            };
            //try
            //{
            MessagePipelineBase pipeline = MatchPipeline(message.Text, ctx);
            ExecutePieplineStage(pipeline, ctx);
            //}
            //catch (Exception ex)
            //{
            //	throw;
            //}

        }

        public void ExecutePieplineStage(MessagePipelineBase pipeline, MessageContext ctx)
        {
            if (pipeline != null)
            {
                string current = _cache.GetValueForChat<string>(CURRENT_MESSAGEPIPELINE_STAGE_NAME, ctx.Recipient);
                bool executeNextImmediately = false;

                var result = current == null ? pipeline.Execute(ctx) : pipeline.Execute(ctx, current);
                if(result != null)
                {
                    if (ctx.PipelineStageSucceeded)
                    {
                        executeNextImmediately = result.InvokeNextImmediately;

                        string nextName = ctx.CurrentStage.NextStage?.MethodName;
                        _cache.SetValueForChat(CURRENT_MESSAGEPIPELINE_STAGE_NAME, nextName, ctx.Recipient);
                    }

                    string commandToSet = null;
                    if (!ctx.PipelineStageSucceeded || ctx.MoveNext)
                    {
                        commandToSet = (pipeline.GetType().GetCustomAttributes(true).FirstOrDefault(attr => (attr as RouteAttribute) != null) as RouteAttribute).Route;
                    }

                    if (pipeline.IsDone)//if it is true - we make null values in cache
                    {
                        _cache.SetValueForChat(CURRENT_MESSAGEPIPELINE_STAGE_NAME, null, ctx.Recipient);
                        _cache.SetValueForChat(CURRENT_MESSAGEPIPELINE_COMMAND, null, ctx.Recipient);
                    }
                    else//else - we leave the command name as it is
                    {
                        _cache.SetValueForChat(CURRENT_MESSAGEPIPELINE_COMMAND, commandToSet, ctx.Recipient);
                    }

                    _sender.SendMessage(result, ctx.Recipient);

                    //if a pipeline signed that a next method should be executed immediately,we invoke again this method
                    if (executeNextImmediately)
                    {
                        ExecutePieplineStage(pipeline, ctx);
                    }
                }
                else
                {
                    _sender.SendMessage(new TextResult("Error! no message pipeline found"), ctx.Recipient);
                }
            }
            else
            {
                _sender.SendMessage(new TextResult("An error occured while handling your message"), ctx.Recipient);
            }
        }

        private MessagePipelineBase MatchPipeline(string text, MessageContext ctx)
        {
            var matchedPipelineType = pipleineCommands.ToList().FirstOrDefault(x => text.Contains(x.Key)).Value;
            if (matchedPipelineType == null)
            {
                var currentCommand = _cache.GetValueForChat<string>(CURRENT_MESSAGEPIPELINE_COMMAND, ctx.Recipient);
                if (currentCommand != null)
                    matchedPipelineType = pipleineCommands.ToList().FirstOrDefault(x => currentCommand.Contains(x.Key)).Value;
            }

            return matchedPipelineType != null ? _lifetimeScope.BeginLifetimeScope().Resolve(matchedPipelineType) as MessagePipelineBase : null;
        }

        private static List<Type> GetPipelineTypes()
        {
            var basePipelineType = typeof(MessagePipelineBase);
            return typeof(StartPipeline).Assembly.GetTypes().Where(t => t.IsSubclassOf(basePipelineType)).ToList();
        }

    }
}
