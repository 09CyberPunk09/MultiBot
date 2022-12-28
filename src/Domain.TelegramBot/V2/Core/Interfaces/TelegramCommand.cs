using Application.TelegramBot.Pipelines.V2.Core.Context;
using Application.TextCommunication.Core.Interfaces;

namespace Application.TelegramBot.Pipelines.V2.Core.Interfaces
{
    public interface ITelegramCommand : ICommand<TelegramMessageContext>
    {
    }
}
