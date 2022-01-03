using Autofac;
using Infrastructure.UI.Core.Types;
using Persistence.Caching.Redis.TelegramCaching;
using System;
using StageDelegate = System.Func<Infrastructure.UI.Core.Types.MessageContext, Infrastructure.UI.Core.Interfaces.ContentResult>;


namespace Infrastructure.UI.Core.MessagePipelines
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

        public void ConfigureBasicPostAction()
        {
            StagePostAction = (Stage stage, MessageContext ctx) =>
            {
                IsDone = false;
                if (stage.NextStage == null)
                {
                    ctx.MoveNext = false;
                    IsDone = true;
                    ctx.PipelineEnded = true;
                }
            };
        }

        protected void RegisterStage(StageDelegate stage)
        {
            Stages.Add(new Stage(stage));
        }
        protected void RegisterStage(Stage stage)
        {
            Stages.Add(stage);
        }
    }
}
