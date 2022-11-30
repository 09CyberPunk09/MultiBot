using Application.Services;
using Application.TelegramBot.Pipelines.IOInstances.Interfaces;
using Infrastructure.Queuing;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.IOInstances
{
    internal class IOHandlerMessageSender : IMessageSender
    {
        private static QueuePublisher _publisher;

        static IOHandlerMessageSender()
        {
            var service = new ConfigurationAppService();
            var queueName = service.Get("Telegram:MessageResponseQueue");
            _publisher = QueuingHelper.CreatePublisher(queueName);
        }

        public void SendMessage(ContentResult message)
        {
            _publisher.Publish(message);
        }

        public async Task SendMessageAsync(ContentResult result)
        {
            await Task.Run(() => SendMessage(result));
        }
    }
}
