using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Middlewares;

namespace TelegramBot.ChatEngine.Implementation.Middlewares
{
    internal interface ITelegramMiddleware : IMiddleware<TelegramMessageContext>
    {
    }
}
