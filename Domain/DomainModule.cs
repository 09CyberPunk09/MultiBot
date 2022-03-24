using Application;
using Autofac;
using System.Linq;

namespace Domain
{
    public class DomainModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            _ = builder.RegisterTypes(GetType().Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(AppService))).ToArray());
            base.Load(builder);
        }
    }
}