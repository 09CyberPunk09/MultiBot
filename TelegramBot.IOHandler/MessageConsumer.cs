using Application.Services;
using Infrastructure.Queuing;
using Infrastructure.TextUI.Core.Types;

namespace TelegramBot.IOHandler
{
    internal class MessageConsumer
    {
        private static QueuePublisher _publisher;

        public MessageConsumer()
        {
            var service = new ConfigurationAppService();
            var hostName = service.Get("telegramQueueHost");
            var queueName = service.Get("telegramHandleMessageQueue");
            var username = service.Get("username");
            var password = service.Get("password");

            _publisher = new QueuePublisher(hostName, queueName, username, password);
        }

        public void ConsumeMessage(Message msg)
        {
            _publisher.Publish(msg);
        }
    }
}