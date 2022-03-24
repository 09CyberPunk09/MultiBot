using Autofac;
using Domain;
using Persistence.Caching.Redis;
using Persistence.Sql;
using System;

namespace Infrastructure.TelegramBot.IOInstances
{
    public class TelegramBotHandlerInstance
    {
        private IContainer _container;

        private MessageHandler _messageConsumer;

        private void ConfigureApplication()
        {
            var containerBuilder = new ContainerBuilder();

            ConfigureHandlersAccess(containerBuilder);

            ConfigurePersistence(containerBuilder);

            ConfigureDomain(containerBuilder);

            ResolveRequiredServices(containerBuilder);
        }

        public void Start()
        {
            ConfigureApplication();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        private void ResolveRequiredServices(ContainerBuilder containerBuilder)
        {
            _container = containerBuilder.Build();

            _messageConsumer = _container.Resolve<MessageHandler>();
        }

        private void ConfigureHandlersAccess(ContainerBuilder containerBuilder)
        {
            _ = containerBuilder.RegisterType<MessageHandler>().SingleInstance();
        }

        private void ConfigurePersistence(ContainerBuilder builder)
        {
            _ = builder.RegisterModule<PersistenceModule>();
            _ = builder.RegisterModule<CachingModule>();
        }

        private void ConfigureDomain(ContainerBuilder builder)
        {
            _ = builder.RegisterModule<PipelinesModule>();
            _ = builder.RegisterModule<DomainModule>();
        }
    }
}
