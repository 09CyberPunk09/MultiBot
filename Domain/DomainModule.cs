using Autofac;
using System.Linq;

namespace Application
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            _ = builder.RegisterTypes(GetType().Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(AppService))).ToArray());
            base.Load(builder);
        }
    }
}