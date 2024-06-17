using TelegramBot.ChatEngine.Commands.Repsonses;
using TelegramBot.ChatEngine.Implementation.Dro;

namespace TelegramBot.ChatEngine.Commands.Interfaces;

public interface IMessageSender
{
    void SendMessage(ContentResultV2 result);
    Task<SentTelegramMessage> SendMessageAsync(ContentResultV2 result);
}
