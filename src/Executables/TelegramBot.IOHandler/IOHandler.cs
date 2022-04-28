using Application.Services;
using Autofac;
using Infrastructure.Queuing;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using Telegram.Bot;

namespace LifeTracker.TelegramBot.IOHandler
{
    public class IOHandler
    {
        private IContainer _container;
        private QueueListener<ContentResult> _queueListenner;
        private MessageSender _sender;
        private readonly Logger logger;
        public IOHandler()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        public void Start()
        {
            var containerBuilder = new ContainerBuilder();
            ConfigureAPIClient(containerBuilder);

            _container = containerBuilder.Build();

            ConfigureMessageSender();
        }

        private void ConfigureAPIClient(ContainerBuilder containerBuilder)
        {
            var configuration = new ConfigurationBuilder()
             .AddJsonFile("appSettings.json").Build();
            var client = new TelegramBotClient(configuration["Telegram:BotAPIKey"])
            {
                Timeout = TimeSpan.FromMinutes(10)
            };
            client.StartReceiving<MessageUpdateHandler>();
            client.GetUpdatesAsync();

            _ = containerBuilder.RegisterInstance(client).As<ITelegramBotClient>().SingleInstance();
            _ = containerBuilder.RegisterType<MessageSender>().SingleInstance();
        }

        private void ConfigureMessageSender()
        {
            var service = new ConfigurationAppService();
            var hostName = service.Get("Telegram:QueueHost");
            var queueName = service.Get("Telegram:MessageResponseQueue");
            var username = service.Get("Telegram:QueueUsername");
            var password = service.Get("Telegram:QueuePassword");

            _sender = _container.Resolve<MessageSender>();
            _queueListenner = new(hostName, queueName, username, password);
            _queueListenner.AddMessageHandler(response =>
            {
                logger.Info($"Message '{response.Text}' sent to { response.RecipientChatId}");
                _sender.SendMessage(response);
            });
            _queueListenner.StartConsuming();
        }
    }
}