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
            var queueName = service.Get("Telegram:HandleMessageQueue");

            _publisher = QueuingHelper.CreatePublisher(queueName);
        }

        public void ConsumeMessage(Message msg)
        {
            _publisher.Publish(msg);
        }
    }
}