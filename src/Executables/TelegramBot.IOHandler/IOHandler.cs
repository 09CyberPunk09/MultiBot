using Application.TextCommunication.Core.Repsonses;
using Autofac;
using Common.Configuration;
using Infrastructure.FileStorage;
using Infrastructure.Queuing;
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

            ConfigurePersistence(containerBuilder);
            var configuration = ConfigurationHelper.GetConfiguration();
            containerBuilder.RegisterInstance(configuration).As<IConfigurationRoot>();

            Container = containerBuilder.Build();

            var client = Container.Resolve<ITelegramBotClient>();
            client.StartReceiving(new MessageUpdateHandler(Container));
            client.GetUpdatesAsync();


            ConfigureMessageSender();
        }
        private void ConfigurePersistence(ContainerBuilder builder)
        {
            _ = builder.RegisterModule(new PersistenceModule());
            _ = builder.RegisterModule<CachingModule>();
            logger.Info("Persistence Configured");

        }
        private void ConfigureFileStorageFunctionality(ContainerBuilder builder)
        {
            builder.RegisterModule<FileStorageModule>();
        }
        private void ConfigureDomain(ContainerBuilder builder)
        {
           // builder.RegisterModule<DomainModule>();
        }

        private void ConfigureAPIClient(ContainerBuilder containerBuilder)
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            var client = new TelegramBotClient(configuration["Telegram:BotAPIKey"])
            {
                Timeout = TimeSpan.FromMinutes(10)
            };
            _ = containerBuilder.RegisterInstance(client).As<ITelegramBotClient>().SingleInstance();
         //   _ = containerBuilder.RegisterType<MessageSender>().SingleInstance();
        }

        private void ConfigureMessageSender()
        {
            //var service = new ConfigurationAppService();
            //var queueName = service.Get("Telegram:MessageResponseQueue");
            //_sender = Container.Resolve<MessageSender>();

            //_queueListenner = QueuingHelper.CreateListener<ContentResult>(queueName);

            //_queueListenner.AddMessageHandler(response =>
            //{
            //    logger.Info($"Message '{response.Text}' sent to { response.RecipientChatId}");
            //    _sender.SendMessage(response);
            //});
            //_queueListenner.StartConsuming();
        }
    }
}