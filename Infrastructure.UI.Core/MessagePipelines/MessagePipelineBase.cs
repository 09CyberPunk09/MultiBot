using Autofac;
using Infrastructure.TextUI.Core.Interfaces;
using Infrastructure.TextUI.Core.Types;
using Persistence.Sql;
using System.Collections.Generic;
using System.Linq;
using StageDelegate = System.Func<Infrastructure.TextUI.Core.Types.MessageContext, Infrastructure.TextUI.Core.Interfaces.ContentResult>;
using SystemUser = Common.Entites.User;

namespace Infrastructure.TextUI.Core.MessagePipelines
{
    public class MessagePipelineBase : Pipeline, IMessagePipeline
    {
        public MessagePipelineBase(ILifetimeScope scope) : base(scope)
        {
            _scope = scope;
        }

        public ContentResult Execute(MessageContext ctx, string stageName = null)
        {
            MessageContext = ctx;
            Stage stage;
            if (stageName != null)
            {
                stage = Stages.Stages.FirstOrDefault(x => x.MethodName == stageName);
            }
            else
            {
                stage = Stages.Stages.FirstOrDefault();
            }

            ctx.CurrentStage = stage;

            var result = stage.Invoke(ctx);
            StagePostAction?.Invoke(stage, ctx);
            MessageContext = null;
            return result;
        }

        protected void IntegrateChunkPipeline<TChunk>() where TChunk : PipelineChunk
        {
            var chunk = _scope.Resolve<TChunk>(new NamedParameter("ctx", MessageContext));
            chunk.Stages.Stages.ForEach(stage => RegisterStage(stage));
        }

        protected SystemUser GetCurrentUser()
        {
            //todo: implement getting from cache
            //todo: add caching library
            using (var ctx = new LifeTrackerDbContext())
            {
                return ctx.Users.FirstOrDefault(u => u.TelegramUserId.HasValue && u.TelegramUserId == MessageContext.Recipient);
            }
        }
    }

    public class StageMap
    {
        public List<Stage> Stages { get; set; } = new();
        public Stage Root { get; set; }

        public Stage this[int index]
        {
            get => Stages[index];
            set => Stages[index] = value;
        }

        public void Add(Stage stage)
        {
            if (Root == null)
            {
                Root = stage;
                Stages.Add(stage);
                return;
            }

            var last = Stages.LastOrDefault();
            last.NextStage = stage;
            Stages.Add(stage);
        }
    }

    public class Stage
    {
        private readonly string _name;

        public Stage(StageDelegate f)
        {
            Method = f;
            _name = f.Method.Name;
        }

        public StageDelegate Method { get; }
        public Stage NextStage { get; set; }
        public string MethodName { get => _name; }

        public ContentResult Invoke(MessageContext ctx)
            => Method.Invoke(ctx);
    }
}