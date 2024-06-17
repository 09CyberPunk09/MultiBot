namespace TelegramBot.ChatEngine.Commands.Interfaces;

public interface IMessageHandler
{
    Task HandleMessage(TelegramMessage message);
}
