using Application.Services;
using Infrastructure.Queuing;
using Infrastructure.TextUI.Core.PipelineBaseKit;

namespace Infrastructure.TelegramBot.IOInstances
{
    public class MessageResponsePublisher
    {
        private static QueuePublisher _publisher;

        static MessageResponsePublisher()
        {
            var service = new ConfigurationAppService();
            var hostName = service.Get("telegramQueueHost");
            var queueName = service.Get("telegramMessageResponseQueue");
            var username = service.Get("username");
            var password = service.Get("password");

            _publisher = new(hostName, queueName, username, password);
        }

        public void SendMessage(ContentResult message)
        {
            _publisher.Publish(message);
        }
    }
}