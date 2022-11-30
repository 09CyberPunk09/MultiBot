using Application;
using Application.TelegramBot.Pipelines.IOInstances;
using Application.TelegramBot.Pipelines.IOInstances.Interfaces;
using Autofac;
using Domain.TelegramBot.Middlewares;
using Infrastructure.FileStorage;
using Kernel;
using Microsoft.Extensions.Configuration;
using NLog;
using Persistence.Caching.Redis;
using Persistence.Master;
using ServiceStack.Messaging;
using System;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.IOInstances
{
    public class TelegramBotHandlerInstance
    {
        IContainer _container;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private MessageHandler _messageHandler;

        private void ConfigureApplication()
        {
            var containerBuilder = new ContainerBuilder();

            var configuration = ConfigurationHelper.GetConfiguration();
            containerBuilder.RegisterInstance(configuration).As<IConfigurationRoot>();

            ConfigureCommunicationInfrastructure(containerBuilder);

            RegisterMiddlewares(containerBuilder);

            ConfigureFileStorageFunctionality(containerBuilder);

            ConfigurePersistence(containerBuilder);

            ConfigureDomain(containerBuilder);

            ResolveRequiredServices(containerBuilder);
        }

        public void Start()
        {
            ConfigureApplication();
        }

        //TODO: Temporary kostil
        private void RegisterMiddlewares(ContainerBuilder builder)
        {
            builder.RegisterType<AuthentificationMiddleware>().InstancePerLifetimeScope();
            builder.RegisterType<UserAllowedFeatureMiddleware>().InstancePerLifetimeScope();
        }
        private void ApplyMiddlewares(MessageHandler messageHandler)
        {
            messageHandler.AddMiddleware<AuthentificationMiddleware>();
            messageHandler.AddMiddleware<UserAllowedFeatureMiddleware>( );
            logger.Info("Configurarion: Message handler middlewares configured");
        }

        private void ResolveRequiredServices(ContainerBuilder containerBuilder)
        {
            _container = containerBuilder.Build();

            _messageHandler = _container.Resolve<MessageHandler>();
            ApplyMiddlewares(_messageHandler);
            _messageHandler.StartReceiving();
        }
        private void ConfigureFileStorageFunctionality(ContainerBuilder builder)
        {
            builder.RegisterModule<FileStorageModule>();
            logger.Info("Configurarion: File storage registered");

        }
        private void ConfigureCommunicationInfrastructure(ContainerBuilder containerBuilder)
        {
            //BUG: Code repeat
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json").Build();
            var client = new TelegramBotClient(configuration["Telegram:BotAPIKey"])
            {
                Timeout = TimeSpan.FromMinutes(10)
            };
            _ = containerBuilder.RegisterInstance(client).As<ITelegramBotClient>().SingleInstance();

            _ = containerBuilder
                .RegisterType<TelegramMessageSender>()
                .As<IMessageSender>()
                .SingleInstance();
            _ = containerBuilder
                .RegisterType<MessageHandler>()
                .SingleInstance();
            logger.Info("Configurarion: Communication infastructure configured");
        }

        private void ConfigurePersistence(ContainerBuilder builder)
        {
            _ = builder.RegisterModule(new PersistenceModule());
            _ = builder.RegisterModule<CachingModule>();
            logger.Info("Configuration: Persistence Configured");

        }

        private void ConfigureDomain(ContainerBuilder builder)
        {
            _ = builder.RegisterModule<PipelinesModule>();
            _ = builder.RegisterModule<DomainModule>();
            logger.Info("Configuration: Domain and pipelines Configured");

        }
    }
}