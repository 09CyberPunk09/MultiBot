using Autofac;
using Persistence.Caching.Redis.TelegramCaching;
using System;
using System.Linq;
using System.Reflection;
using Telegram.Bot.Types.ReplyMarkups;
using CallbackButtonButton = Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton;
using StageDelegate = System.Func<Infrastructure.TextUI.Core.PipelineBaseKit.ContentResult>;
using AsyncStageDelegate = System.Func<Infrastructure.TextUI.Core.PipelineBaseKit.ContentResult>;
using System.Collections.Generic;
using TextUI.Core.PipelineBaseKit;

namespace Infrastructure.TextUI.Core.PipelineBaseKit
{
    public class Pipeline
    {
        public Response Response { get; set; } = new();
        public StageMap Stages { get; set; }
        public Dictionary<string, StageDelegate> Delegates { get; set; } = new();
        public List<AsyncStageDelegate> AsyncDelegates { get; set; }

        protected TelegramCache cache = new();

        public MessageContext MessageContext { get; set; }

        public bool IsLooped { get; set; }
        public Action<Stage, MessageContext, ContentResult> StagePostAction { get; set; }
        public ILifetimeScope _scope { get; set; }

        public Pipeline(ILifetimeScope scope)
        {
            _scope = scope;
            Stages = new(this);
            InitBaseComponents();
            RegisterPipelineStages();
        }

        public PipelineExecutionResponse Execute(MessageContext ctx,Type pipelineType, int stageIndex = -1)
        {
            Stage stage;
            stage = stageIndex != -1 ? Stages[stageIndex] : Stages.Root;
            ContentResult result;
            if(stage.OverridedStage != null)
            {
                var chunkType = GetType().Assembly.GetTypes().FirstOrDefault(x => x.FullName == stage.OverridedStage.TypeFullName);

                var chunk = _scope.Resolve(chunkType) as Pipeline;
                chunk.MessageContext = MessageContext;
                var res = chunk.Execute(MessageContext, chunkType, stage.OverridedStage.Index);
                res.Result.RecipientChatId = MessageContext.RecipientChatId;
                bool pipelineEnded = stage.NextStage == null;
                return new PipelineExecutionResponse()
                {
                    Result = res.Result,
                    CanIvokeNext = res.CanIvokeNext,
                    DeleteLastBotMessage = res.DeleteLastBotMessage,
                    DeleteLastUserMessage = res.DeleteLastUserMessage,
                    PipelineEnded = pipelineEnded,
                    NextPipelineTypeFullName = res.NextPipelineTypeFullName == null ? stage.NextStage.TypeFullName : res.NextPipelineTypeFullName,
                    NextStageIndex = pipelineEnded ? -1 : stage.NextStage.Index,
                };
            }
            else
            {
                bool pipelineEnded  = Response.PipelineEnded ? Response.PipelineEnded : stage.NextStage == null;
                if (stage.IsLambdaExpr)
                {
                    result = Delegates[stage.MethodName].Invoke();
                }
                else
                {
                    result = pipelineType.GetMethod(stage.MethodName,
                        BindingFlags.Instance |
                        BindingFlags.IgnoreCase |
                        BindingFlags.Public |
                        BindingFlags.NonPublic).Invoke(this, null) as ContentResult;
                }
                result.RecipientChatId = MessageContext.RecipientChatId;
                return new PipelineExecutionResponse()
                {
                    Result = result,
                    CanIvokeNext = Response.CanIvokeNext,
                    DeleteLastBotMessage = Response.DeleteLastBotMessage,
                    DeleteLastUserMessage = Response.DeleteLastUserMessage,
                    PipelineEnded = pipelineEnded,
                    NextPipelineTypeFullName = pipelineEnded ? null : stage.NextStage.TypeFullName,
                    NextStageIndex = pipelineEnded ? -1 : stage.NextStage.Index,
                };
            }
        }

        public virtual void RegisterPipelineStages()
        {

        }

        protected static RouteAttribute GetRoute<TPpileline>() where TPpileline : Pipeline
        {
            var attr = typeof(TPpileline).GetCustomAttribute(typeof(RouteAttribute)) as RouteAttribute;
            return attr;
        }

        protected static string GetAlternativeRoute<TPpileline>() where TPpileline : Pipeline
            => GetRoute<TPpileline>().AlternativeRoute;

        protected void InitBaseComponents()
        {
            ConfigureBasicPostAction();
        }

        public virtual void ConfigureBasicPostAction()
        {
            StagePostAction += (Stage stage, MessageContext ctx, ContentResult result) =>
            {
                if (result.Menu != null)
                {
                    var temp = result.Menu.Keyboard.ToList();
                    temp.Add(new[] { new KeyboardButton("🏡Home") });
                    result.Menu.Keyboard = temp;
                }
            };

            StagePostAction += (stage, ctx, result) =>
            {
                //IsDone = false;
                //if (stage.NextStage == null && ctx.PipelineStageSucceeded)
                //{
                //    ctx.MoveNext = false;
                //    IsDone = true;
                //    ctx.PipelineEnded = true;
                //}
            };
        }

        protected CallbackButtonButton Button(string text, string callbackData)
            => CallbackButtonButton.WithCallbackData(text, callbackData);

        protected void RegisterStage(string stageName)
        {
            Stages.Add(stageName);
        }
        protected void RegisterStage(StageDelegate @delegate)
        {
            Stages.Add(@delegate);
        }
        public void IntegrateChunkPipeline<TPipeline>() where TPipeline : PipelineChunk
        {
            var chunk = _scope.Resolve<TPipeline>();
            var currentTypeFullName = GetType().FullName;
            chunk.Stages.Stages.ForEach(x =>
            {
                Stages.Add(new Stage(x.MethodName, currentTypeFullName)
                {
                   OverridedStage = x    
                });
            });
        }

        public void RegisterStageMethod(StageDelegate @delegate)
        {
            string methodName = @delegate.Method.Name;
            Stages.Add(methodName);
        }

        #region ResponseTemplates

        protected ContentResult Text(string text, bool invokeNextImmediately = false) => ResponseTemplates.Text(text, invokeNextImmediately);

        #endregion ResponseTemplates

        protected T GetCachedValue<T>(string key, long chatId,bool getThanDelete = false)
            => cache.GetValueForChat<T>(key, chatId,getThanDelete);

        protected void SetCachedValue(string key, object value, long chatId)
            => cache.SetValueForChat(key, value, chatId);
    }
}