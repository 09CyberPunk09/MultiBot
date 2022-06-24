using Autofac;

namespace Persistence.Caching.Redis
{
    public class CachingModule : Autofac.Module
    {
        //TODO: Implelemnt cache logging
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Cache>().As<ICache>().SingleInstance();
            base.Load(builder);
        }
    }
}