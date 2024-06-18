using Common.Configuration;
using Infrastructure.Queuing;
using Infrastructure.Queuing.Core;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Implementation;

namespace TelegramBot.ChatEngine.Infrastructure;

public class MessageReceiver
{
    //NOW: REMOVE MESSAGE INCOMMING STRATEGY AWAY FROM LIBRARY PROJECT
    private QueueListener<TelegramMessage> _listener;
    private readonly TelegramBotMessageHandler _messageHandler;
    public MessageReceiver(TelegramBotMessageHandler handler)
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