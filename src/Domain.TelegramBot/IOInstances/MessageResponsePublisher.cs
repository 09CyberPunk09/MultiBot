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
            var queueName = service.Get("Telegram:MessageResponseQueue");
            _publisher = QueuingHelper.CreatePublisher(queueName);
        }

        public void SendMessage(ContentResult message)
        {
            _publisher.Publish(message);
        }
    }
}