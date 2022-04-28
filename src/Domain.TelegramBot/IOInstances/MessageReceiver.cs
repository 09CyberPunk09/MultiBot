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
            var hostName = service.Get("Telegram:QueueHost");
            var queueName = service.Get("Telegram:HandleMessageQueue");
            var username = service.Get("Telegram:QueueUsername");
            var password = service.Get("Telegram:QueuePassword");

            _listener = new(hostName, queueName, username, password);
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