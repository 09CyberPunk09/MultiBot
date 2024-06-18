using Autofac;
using Common.Configuration;
using Infrastructure.Queuing.Core;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using Telegram.Bot;
using TelegramBot.ChatEngine.Commands.Repsonses;

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

            var configuration = ConfigurationHelper.GetConfiguration();
            containerBuilder.RegisterInstance(configuration).As<IConfigurationRoot>();

            Container = containerBuilder.Build();

            var client = Container.Resolve<ITelegramBotClient>();
            client.StartReceiving(new MessageUpdateHandler(Container));
            client.GetUpdatesAsync();
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