using Application.TelegramBot.Pipelines.Old.IOInstances;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Persistence.Master;
using System.Linq;

namespace Application.TelegramBot.Pipelines.Old
{
    public class PipelinesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var typesToRegister = GetType().Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(MessagePipelineBase))).ToList();
            typesToRegister.AddRange(GetType().Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(PipelineChunk))));

            //typesToRegister.ForEach(type =>
            //{
            //    builder.RegisterType(type).OnActivating(e =>
            //    {
            //        var dep = e.Context.Resolve<ILifetimeScope>();
            //        ((Pipeline)e.Instance).InitLifeTimeScope(dep);
            //    }).InstancePerLifetimeScope();
            //});

            _ = builder.RegisterTypes(GetType().Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(MessagePipelineBase))).ToArray()).InstancePerLifetimeScope();
            _ = builder.RegisterTypes(GetType().Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(PipelineChunk))).ToArray()).InstancePerLifetimeScope();

            _ = builder.RegisterModule(new PersistenceModule());
            // _ = builder.RegisterModule<DomainModule>();
            _ = builder.RegisterType<MiddlewareHandler>().SingleInstance();

            base.Load(builder);
        }
    }
}