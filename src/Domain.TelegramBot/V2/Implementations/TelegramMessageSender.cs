using Application.Telegram.Implementations;
using Application.TextCommunication.Core.Interfaces;
using Application.TextCommunication.Core.Repsonses;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Application.TelegramBot.Pipelines.V2.Implementations;

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
