using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TelegramBot.Pipelines.V2.Core.Middlewares;
using System;

namespace Application.TelegramBot.Pipelines.V2.Implementations.Middlewares;

internal sealed class TelegramMiddlewareHandler : BaseMiddlewareHandler<TelegramMessageContext>
{
    public TelegramMiddlewareHandler(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
