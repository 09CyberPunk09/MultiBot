using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Interfaces;

namespace TelegramBot.ChatEngine.Infrastructure
{
    internal class MessageReceiver
    {
        //NOW: REMOVE MESSAGE INCOMMING STRATEGY AWAY FROM LIBRARY PROJECT
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