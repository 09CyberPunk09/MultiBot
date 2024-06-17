using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Middlewares;

namespace TelegramBot.ChatEngine.Implementation.Middlewares;

internal sealed class TelegramMiddlewareHandler : BaseMiddlewareHandler<TelegramMessageContext>
{
    public TelegramMiddlewareHandler(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
