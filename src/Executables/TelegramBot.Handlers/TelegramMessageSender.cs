using Application.Telegram.Implementations;
using Telegram.Bot;
using TelegramBot.ChatEngine.Commands.Interfaces;
using TelegramBot.ChatEngine.Commands.Repsonses;
using TelegramBot.ChatEngine.Implementation.Dro;

namespace TelegramBot.ChatEngine.Infrastructure;

public class TelegramMessageSender : IMessageSender
{
    private readonly ITelegramBotClient _client;
    public TelegramMessageSender(ITelegramBotClient client)
    {
        _client = client;
    }
    public void SendMessage(ContentResultV2 result)
    {
        var strategy = new MessageSendingStrategy(_client);
        strategy.SendMessage(result).Wait();
    }

    public async Task<SentTelegramMessage> SendMessageAsync(ContentResultV2 result)
    {
        var strategy = new MessageSendingStrategy(_client);
        var message = await strategy.SendMessage(result);
        return new SentTelegramMessage()
        {
            SentMessage = message
        };
    }
}
