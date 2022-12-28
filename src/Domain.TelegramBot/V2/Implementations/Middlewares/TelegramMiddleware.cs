using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TelegramBot.Pipelines.V2.Core.Middlewares.Interfaces;

namespace Application.TelegramBot.Pipelines.V2.Implementations.Middlewares
{
    internal interface ITelegramMiddleware : IMiddleware<TelegramMessageContext>
    {
    }
}
