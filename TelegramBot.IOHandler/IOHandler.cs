using Application.Services;
using Autofac;
using Infrastructure.Queuing;
using Infrastructure.TextUI.Core.Interfaces;
using System;
using Telegram.Bot;

namespace TelegramBot.IOHandler
{
    public class IOHandler
    {
        private IContainer _container;
        private QueueListener<ContentResult> _queueListenner;
        private MessageSender _sender;

        public void Start()
        {
            var containerBuilder = new ContainerBuilder();
            ConfigureAPIClient(containerBuilder);

            _container = containerBuilder.Build();

            ConfigureMessageSender();
        }

        private void ConfigureAPIClient(ContainerBuilder containerBuilder)
        {
            var client = new TelegramBotClient("1740254100:AAGW32c6AWAqilo1xNYLUim5zsgTXn8g9x4")
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
            var hostName = service.Get("telegramQueueHost");
            var queueName = service.Get("telegramMessageResponseQueue");
            var username = service.Get("username");
            var password = service.Get("password");

            _sender = _container.Resolve<MessageSender>();
            _queueListenner = new(hostName, queueName, username, password);
            _queueListenner.AddMessageHandler(response =>
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                _sender.SendMessage(response);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            });
            _queueListenner.StartConsuming();
        }
    }
}