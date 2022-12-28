using Application.Chatting.Core.Interfaces;
using Application.TelegramBot.Commands.Core.Context;

namespace Application.TelegramBot.Commands.Core.Interfaces
{
    public interface ITelegramStage : IStage<TelegramMessageContext>
    {
    }
}
