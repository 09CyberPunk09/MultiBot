using Application.Chatting.Core.Repsonses;
using Autofac;
using Common.Configuration;
using Infrastructure.FileStorage;
using Infrastructure.Queuing;
using Infrastructure.Queuing.Core;
using Microsoft.Extensions.Configuration;
using NLog;
using Persistence.Caching.Redis;
using Persistence.Master;
using System;
using Telegram.Bot;

namespace LifeTracker.TelegramBot.IOHandler
{
    public class IOHandler
    {
        public IContainer Container { get; set; }
        private QueueListener<ContentResultV2> _queueListenner;
        private readonly Logger logger;
        public IOHandler()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        public void Start()
        {
            var containerBuilder = new ContainerBuilder();

            ConfigureAPIClient(containerBuilder);

            ConfigureFileStorageFunctionality(containerBuilder);

            ConfigureDomain(containerBuilder);

            var configuration = ConfigurationHelper.GetConfiguration();
            containerBuilder.RegisterInstance(configuration).As<IConfigurationRoot>();

            Container = containerBuilder.Build();

            var client = Container.Resolve<ITelegramBotClient>();
            client.StartReceiving(new MessageUpdateHandler(Container));
            client.GetUpdatesAsync();
        }
    
        private void ConfigureFileStorageFunctionality(ContainerBuilder builder)
        {
            builder.RegisterModule<FileStorageModule>();
        }
        private void ConfigureDomain(ContainerBuilder builder)
        {
        }

        private void ConfigureAPIClient(ContainerBuilder containerBuilder)
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            var client = new TelegramBotClient(configuration["Telegram:BotAPIKey"])
            {
                Timeout = TimeSpan.FromMinutes(10)
            };
            _ = containerBuilder.RegisterInstance(client).As<ITelegramBotClient>().SingleInstance();
        }

    }
}