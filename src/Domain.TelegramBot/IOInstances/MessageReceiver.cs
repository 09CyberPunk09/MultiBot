using Application.Services;
using Infrastructure.Queuing;
using Infrastructure.TextUI.Core.PipelineBaseKit;

namespace Infrastructure.TelegramBot.IOInstances
{
    internal class MessageReceiver
    {
        private QueueListener<Message> _listener;
        private readonly MessageHandler _messageHandler;

        public MessageReceiver(MessageHandler handler)
        {
            _messageHandler = handler;

            var service = new ConfigurationAppService();
            var queueName = service.Get("Telegram:HandleMessageQueue");

            _listener = QueuingHelper.CreateListener<Message>(queueName);
        }

        public void StartReceiving()
        {
            _listener.AddMessageHandler(message =>
            {
                _messageHandler.ConsumeMessage(message);
            });
            _listener.StartConsuming();
        }
    }
}