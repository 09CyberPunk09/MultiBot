using Application.TextCommunication.Core.Interfaces;
using Common.Configuration;
using Infrastructure.Queuing;
using Infrastructure.TextUI.Core.PipelineBaseKit;

namespace Application.TelegramBot.Pipelines.V2.Implementations
{
    internal class MessageReceiver
    {
        private QueueListener<TelegramMessage> _listener;
        private readonly IMessageHandler _messageHandler;

        public MessageReceiver(IMessageHandler handler)
        {
            _messageHandler = handler;

            var config = ConfigurationHelper.GetConfiguration();
            var queueName = config["Telegram:HandleMessageQueue"];

            _listener = QueuingHelper.CreateListener<TelegramMessage>(queueName);
        }

        public void StartReceiving()
        {
            _listener.AddMessageHandler(message =>
            {
                _messageHandler.HandleMessage(message);
            });
            _listener.StartConsuming();
        }
    }
}