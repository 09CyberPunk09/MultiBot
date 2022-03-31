using Autofac;
using Persistence.Caching.Redis.TelegramCaching;
using System;
using System.Reflection;
using CallbackButtonButton = Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton;
using StageDelegate = System.Func<Infrastructure.TextUI.Core.PipelineBaseKit.MessageContext, Infrastructure.TextUI.Core.PipelineBaseKit.ContentResult>;

namespace Infrastructure.TextUI.Core.PipelineBaseKit
{
    public class Pipeline
    {
        public StageMap Stages { get; set; } = new();

        //todo: review which type of cache is inited in this project

        protected TelegramCache cache = new();

        public MessageContext MessageContext { get; set; }

        public bool IsLooped { get; set; }
        public Action<Stage, MessageContext> StagePostAction { get; set; }
        public int CurrentActionIndex { get; set; }
        public bool IsDone { get; set; }
        public ILifetimeScope _scope { get; set; }

        public void InitLifeTimeScope(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public Pipeline(ILifetimeScope scope)
        {
            _scope = scope;
            InitBaseComponents();
            RegisterPipelineStages();
        }

        public Pipeline()
        {

        }

        public virtual void RegisterPipelineStages()
        {
        }

        protected static string GetCommand<TPpileline>() where TPpileline : Pipeline
        {
            var attr = typeof(TPpileline).GetCustomAttribute(typeof(RouteAttribute)) as RouteAttribute;
            return attr.Route;
        }

        protected void InitBaseComponents()
        {
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

        #endregion ResponseTemplates

        protected T GetCachedValue<T>(string key, long chatId)
            => cache.GetValueForChat<T>(key, chatId);

        protected void SetCachedValue(string key, object value, long chatId)
            => cache.SetValueForChat(key, value, chatId);
    }
}