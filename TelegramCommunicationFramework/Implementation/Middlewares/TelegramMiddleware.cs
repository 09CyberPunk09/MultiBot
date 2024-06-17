using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Interfaces;

namespace TelegramBot.ChatEngine.Implementation.Middlewares
{
    internal interface ITelegramMiddleware : IMiddleware<TelegramMessageContext>
    {
    }
}
