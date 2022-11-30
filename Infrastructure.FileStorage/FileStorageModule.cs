using Autofac;
using Infrastructure.FileStorage.Implementations;

namespace Infrastructure.FileStorage
{
    public class FileStorageModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LocalStorageRepository>().InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
