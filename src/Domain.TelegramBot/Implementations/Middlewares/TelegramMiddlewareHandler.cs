using Application.Chatting.Core.Middlewares;
using Application.TelegramBot.Commands.Core.Context;
using System;

namespace Application.TelegramBot.Commands.Implementations.Middlewares;

internal sealed class TelegramMiddlewareHandler : BaseMiddlewareHandler<TelegramMessageContext>
{
    public TelegramMiddlewareHandler(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
