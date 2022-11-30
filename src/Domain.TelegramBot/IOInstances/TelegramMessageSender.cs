using Application.Telegram.Implementations;
using Application.TelegramBot.Pipelines.IOInstances.Interfaces;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Application.TelegramBot.Pipelines.IOInstances;

public class TelegramMessageSender : IMessageSender
{
    private readonly ITelegramBotClient _client;
    public TelegramMessageSender(ITelegramBotClient client)
    {
        _client = client;
    }
    public void SendMessage(ContentResult result)
    {
        var strategy = new MessageSendingStrategy(_client);
        strategy.SendMessage(result).Wait();
    }
    public async Task SendMessageAsync(ContentResult result)
    {
        var strategy = new MessageSendingStrategy(_client);
        await strategy.SendMessage(result);
    }
}
