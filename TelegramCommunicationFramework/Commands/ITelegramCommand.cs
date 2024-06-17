using TelegramBot.ChatEngine.Commands.Interfaces;

namespace TelegramBot.ChatEngine.Commands
{
    /// <summary>
    /// Type definition simplification
    /// </summary>
    public interface ITelegramCommand : ICommand<TelegramMessageContext>
    {
    }
}
