using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;
using StageDelegate = System.Func<Infrastructure.TextUI.Core.PipelineBaseKit.ContentResult>;

namespace Infrastructure.TextUI.Core.PipelineBaseKit
{
    //TODO: Divide inner class logic into different classes
    public class MessagePipelineBase : Pipeline, IMessagePipeline
    {
        //TODO: https://autofac.readthedocs.io/en/latest/register/prop-method-injection.html
        public MessagePipelineBase(ILifetimeScope scope) : base(scope)
        {
            _scope = scope;
        }

        protected InlineKeyboardButton Button(string content, string data)
            => InlineKeyboardButton.WithCallbackData(content, data);
        protected List<InlineKeyboardButton> ButtonRow(string content, string data)
            => new() { InlineKeyboardButton.WithCallbackData(content, data) };

        protected KeyboardButton MenuButton(string text)
            => new(text);
        protected KeyboardButton[] MenuButtonRow(string text)
            => new[] { new KeyboardButton(text) };
        protected KeyboardButton[] MenuButtonRow(params KeyboardButton[] butotns)
            => butotns;

        protected T GetCachedValue<T>(string key, bool getThanDelete = false)
            => cache.GetValueForChat<T>(key, MessageContext.RecipientChatId, getThanDelete);

        protected void SetCachedValue(string key, object value)
            => cache.SetValueForChat(key, value, MessageContext.RecipientChatId);

    }

    public class StageMap
    {
        public Stage this[int index]
        {
            get { return Stages[index]; }
        }
        public StageMap(Pipeline pipeline)
        {
            Pipeline = pipeline;
        }
        public Pipeline Pipeline { get; set; }
        public Type PipelineType => Pipeline.GetType();
        public int MethodNameCounter { get; set; } = 0;
        public List<Stage> Stages { get; set; } = new();
        public Stage Root { get; set; }

        public void Add(StageDelegate @delegate)
        {
            var typeName = PipelineType.FullName;
            var delegateName = (++MethodNameCounter).ToString();
            Pipeline.Delegates.TryAdd(delegateName, @delegate);
            var stage = new Stage(true, delegateName, typeName);
            Add(stage);
        }   
        public void Add(string methodName)
        {
            var typeName = PipelineType.FullName;
            var stage = new Stage(methodName, typeName);
            Add(stage);
        }

        public void Add(Stage stage)
        {
            if (Root == null)
            {
                Root = stage;
                Stages.Add(stage);
                stage.Index = Stages.Count - 1;
                return;
            }
            if(Root != null && Stages.Count == 1)
            {
                Root.NextStage = stage;
                Stages.Add(stage);
                stage.Index = Stages.Count - 1;
                return;
            }

            var last = Stages.LastOrDefault();
            Stages.Add(stage);
            stage.Index = Stages.Count - 1;
            last.NextStage = stage;

        }
    }

    public class Stage
    {

        public Stage(string methodName, string typeFullName)
        {
            MethodName = methodName;
            TypeFullName = typeFullName;
        }
        public Stage(bool isLambdaExpr, string name, string typeFullName)
        {
            IsLambdaExpr = true;
            MethodName = name;
            TypeFullName = typeFullName;
        }
        public int Index { get; set; }
        public Stage OverridedStage { get; set; }
        public string TypeFullName { get; set; }
        public string MethodName { get; }
        public bool IsAsync { get; set; }
        public bool IsLambdaExpr { get; set; }
        public Stage NextStage { get; set; }
    }
}