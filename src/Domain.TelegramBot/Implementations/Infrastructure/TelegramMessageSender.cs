using Application.Chatting.Core.Interfaces;
using Application.Chatting.Core.Repsonses;
using Application.Telegram.Implementations;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Application.TelegramBot.Commands.Implementations.Infrastructure;

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
    public async Task SendMessageAsync(ContentResultV2 result)
    {
        var strategy = new MessageSendingStrategy(_client);
        await strategy.SendMessage(result);
    }
}
