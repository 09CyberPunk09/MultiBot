using Autofac;

namespace Persistence.Caching.Redis
{
    public class CachingModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Cache>().As<ICache>().SingleInstance();
            base.Load(builder);
        }
    }
}