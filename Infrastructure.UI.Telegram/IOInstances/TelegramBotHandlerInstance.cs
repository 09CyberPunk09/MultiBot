using Application;
using Autofac;
using NLog;
using Persistence.Caching.Redis;
using Persistence.Sql;
using System;

namespace Infrastructure.TelegramBot.IOInstances
{
    public class TelegramBotHandlerInstance
    {
         IContainer _container;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

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
            logger.Info("Handler Access Configured");
        }

        private void ConfigurePersistence(ContainerBuilder builder)
        {
            _ = builder.RegisterModule<PersistenceModule>();
            _ = builder.RegisterModule<CachingModule>();
            logger.Info("Persistence Configured");

        }

        private void ConfigureDomain(ContainerBuilder builder)
        {
            _ = builder.RegisterModule<PipelinesModule>();
            _ = builder.RegisterModule<DomainModule>();
            logger.Info("Domain and pipelines Configured");

        }
    }
}