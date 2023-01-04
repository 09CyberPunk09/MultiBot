using Application.Chatting.Core.Interfaces;
using Application.Chatting.Core.Messaging;
using Application.Chatting.Core.Repsonses;
using Application.Telegram.Implementations;
using Application.TelegramBot.Commands.Core;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Application.TelegramBot.Commands.Implementations.Infrastructure;

public class TelegramMessageSender : IMessageSender<SentTelegramMessage>
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
