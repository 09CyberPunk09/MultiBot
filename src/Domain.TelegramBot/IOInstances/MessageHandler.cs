﻿using Autofac;
using Infrastructure.TelegramBot.MessagePipelines;
using Infrastructure.TelegramBot.ResponseTypes;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using NLog;
using Persistence.Caching.Redis.TelegramCaching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Infrastructure.TelegramBot.IOInstances
{
    public class MessageHandler
    {
        #region Cache Keys

        private const string CURRENT_MESSAGEPIPELINE_STAGE_NAME = "CurrntPipelineStageName";
        public const string CURRENT_MESSAGEPIPELINE_COMMAND = "CurrentMessagePipelineCommand";

        #endregion Cache Keys

        #region Injected members

        private readonly MessageResponsePublisher _sender;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly TelegramCache _cache;
        private MessageReceiver _receiver;

        #endregion Injected members
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly Dictionary<RouteAttribute, Type> pipleineCommands;
        public IMessagePipeline DefaultPipeline { get; set; }

        public MessageHandler(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
            _sender = new();
            _cache = new();

            StartReceiving();
        }

        private void StartReceiving()
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
            MessageContext ctx = new()
            {
                Message = message,
                MoveNext = true,
                Recipient = message.ChatId,
                TimeStamp = DateTime.Now
            };

            MessagePipelineBase pipeline = MatchPipeline(message.Text, ctx);
            ExecutePieplineStage(pipeline, ctx);
        }

        private void ExecutePieplineStage(MessagePipelineBase pipeline, MessageContext ctx)
        {
            if (pipeline != null)
            {
                string current = _cache.GetValueForChat<string>(CURRENT_MESSAGEPIPELINE_STAGE_NAME, ctx.Recipient);
                bool executeNextImmediately = false;

                var routeName = pipeline.GetType().GetCustomAttribute<RouteAttribute>().Route;
                ContentResult result;
                try
                {
                    result = current == null ? pipeline.Execute(ctx) : pipeline.Execute(ctx, current);
                    logger.Info($"{routeName}.{current}: Message {ctx.Message} processed");

                    if (result != null)
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
                            commandToSet = GetRoute(pipeline);
                        }

                        if (pipeline.IsDone)//if it is true - we make null values in cache
                        {
                            PurgePipelineInfo(ctx.Recipient);
                        }
                        else//else - we leave the command name as it is
                        {
                            _cache.SetValueForChat(CURRENT_MESSAGEPIPELINE_COMMAND, commandToSet, ctx.Recipient);
                        }

                        result.RecipientChatId = ctx.Recipient;
                        _sender.SendMessage(result);

                        //if a pipeline signed that a next method should be executed immediately,we invoke again this method
                        if (executeNextImmediately)
                        {
                            ExecutePieplineStage(pipeline, ctx);
                        }
                    }
                    else
                    {
                        _sender.SendMessage(new TextResult("Error! no message pipeline found", ctx.Recipient));
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex,ctx);
                    throw;
                }
            }
            else
            {
                _sender.SendMessage(new TextResult("No corresponding pipeline found", ctx.Recipient));
            }
        }

        private MessagePipelineBase MatchPipeline(string text, MessageContext ctx)
        {
            //TODO: BUG: Example: /create_todo and /create_todo_category conflict
            var matchedPipelineType = pipleineCommands
                .ToList()
                .FirstOrDefault(x => text.Contains(x.Key.Route) || (x.Key.AlternativeRoute != null && text.Contains(x.Key.AlternativeRoute))).Value;
            if (matchedPipelineType == null)
            {
                var currentCommand = _cache.GetValueForChat<string>(CURRENT_MESSAGEPIPELINE_COMMAND, ctx.Recipient);
                if (currentCommand != null)
                    matchedPipelineType = pipleineCommands.ToList().FirstOrDefault(x => (x.Key.AlternativeRoute != null && currentCommand.Contains(x.Key.AlternativeRoute)) || currentCommand.Contains(x.Key.Route)).Value;
            }

            return matchedPipelineType != null ? _lifetimeScope.BeginLifetimeScope().Resolve(matchedPipelineType) as MessagePipelineBase : null;
        }

        private static List<Type> GetPipelineTypes()
        {
            var basePipelineType = typeof(MessagePipelineBase);
            return typeof(StartPipeline).Assembly.GetTypes().Where(t => t.IsSubclassOf(basePipelineType)).ToList();
        }


        #region service methods
        private void HandleException(Exception ex, MessageContext ctx)
        {
            logger.Error(ex);
            //TODO: In future,move any message sending logics to pipelines
            _sender.SendMessage(new TextResult($"An exception occured while processing your message. Exception: {ex.Message}. {Environment.NewLine} Stack Trace: {ex.StackTrace}"));
            PurgePipelineInfo(ctx.Recipient);
            _sender.SendMessage(new TextResult("Rolled back last command. Please, try again."));
        }

        private void PurgePipelineInfo(long recipientId)
        {
            _cache.SetValueForChat(CURRENT_MESSAGEPIPELINE_STAGE_NAME, null, recipientId);
            _cache.SetValueForChat(CURRENT_MESSAGEPIPELINE_COMMAND, null, recipientId);
        }

        private string GetRoute(MessagePipelineBase pipeline)
            => (pipeline.GetType().GetCustomAttributes(true).FirstOrDefault(attr => attr as RouteAttribute != null) as RouteAttribute).Route;

        #endregion
    }
}