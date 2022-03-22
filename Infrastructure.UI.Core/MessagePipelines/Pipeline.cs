using Autofac;
using Infrastructure.TextUI.Core.Interfaces;
using Infrastructure.TextUI.Core.Types;
using Persistence.Caching.Redis.TelegramCaching;
using System;
using CallbackButtonButton = Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton;
using StageDelegate = System.Func<Infrastructure.TextUI.Core.Types.MessageContext, Infrastructure.TextUI.Core.Interfaces.ContentResult>;

namespace Infrastructure.TextUI.Core.MessagePipelines
{
    public class Pipeline
    {
        public StageMap Stages { get; set; }

        //todo: review which type of cache is inited in this project

        protected TelegramCache cache = new();

        public MessageContext MessageContext { get; set; }

        public bool IsLooped { get; set; }
        public Action<Stage, MessageContext> StagePostAction { get; set; }
        public int CurrentActionIndex { get; set; }
        public bool IsDone { get; set; }
        protected ILifetimeScope _scope;

        public Pipeline(ILifetimeScope scope)
        {
            _scope = scope;
            InitBaseComponents();
            RegisterPipelineStages();

        }
        public virtual void RegisterPipelineStages()
        {

        }

        protected void InitBaseComponents()
        {
            Stages = new();
            ConfigureBasicPostAction();
        }

        public void ForbidMovingNext()
        {
            MessageContext.MoveNext = false;
            MessageContext.PipelineStageSucceeded = false;
            MessageContext.PipelineEnded = false;
            IsDone = false;
        }
        public void ForbidMovingNext(MessageContext ctx)
        {
            ctx.MoveNext = false;
            ctx.PipelineStageSucceeded = false;
            ctx.PipelineEnded = false;
            IsDone = false;
        }

        public virtual void ConfigureBasicPostAction()
        {
            StagePostAction = (stage, ctx) =>
            {
                IsDone = false;
                if (stage.NextStage == null && ctx.PipelineStageSucceeded)
                {
                    ctx.MoveNext = false;
                    IsDone = true;
                    ctx.PipelineEnded = true;
                }
            };
        }

        protected CallbackButtonButton Button(string text, string callbackData)
            => CallbackButtonButton.WithCallbackData(text, callbackData);

        protected void RegisterStage(StageDelegate stage)
        {
            Stages.Add(new Stage(stage));
        }
        protected void RegisterStage(Stage stage)
        {
            Stages.Add(stage);
        }

        #region ResponseTemplates
        protected ContentResult Text(string text, bool invokeNextImmediately = false) => ResponseTemplates.Text(text, invokeNextImmediately);
        #endregion

        protected T GetCachedValue<T>(string key, long chatId)
            => cache.GetValueForChat<T>(key, chatId);
        protected void SetCachedValue(string key, object value, long chatId)
            => cache.SetValueForChat(key, value, chatId);

    }
}
