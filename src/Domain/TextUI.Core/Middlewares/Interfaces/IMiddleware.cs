using Application.TextCommunication.Core.Context;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.V2.Core.Middlewares.Interfaces
{
    public interface IMiddleware<TContext> where TContext: MessageContext
    {
        Task<bool> ExecuteAsync(TContext context);
    }
}
