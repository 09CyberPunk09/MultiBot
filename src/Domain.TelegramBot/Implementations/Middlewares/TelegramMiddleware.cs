using Application.Chatting.Core.Middlewares.Interfaces;
using Application.TelegramBot.Commands.Core.Context;

namespace Application.TelegramBot.Commands.Implementations.Middlewares
{
    internal interface ITelegramMiddleware : IMiddleware<TelegramMessageContext>
    {
    }
}
