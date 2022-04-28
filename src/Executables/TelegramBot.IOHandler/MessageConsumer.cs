using Application.Services;
using Infrastructure.Queuing;
using Infrastructure.TextUI.Core.PipelineBaseKit;

namespace LifeTracker.TelegramBot.IOHandler
{
    internal class MessageConsumer
    {
        private static QueuePublisher _publisher;

        public MessageConsumer()
        {
            var service = new ConfigurationAppService();
            var hostName = service.Get("Telegram:QueueHost");
            var queueName = service.Get("Telegram:HandleMessageQueue");
            var username = service.Get("Telegram:QueueUsername");
            var password = service.Get("Telegram:QueuePassword");

            _publisher = new QueuePublisher(hostName, queueName, username, password);
        }

        public void ConsumeMessage(Message msg)
        {
            _publisher.Publish(msg);
        }
    }
}