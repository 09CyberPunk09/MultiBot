using System.Threading.Tasks;

namespace TelegramBot.ChatEngine.Commands.Interfaces
{
    public interface IMiddleware<TContext> where TContext : TelegramMessageContext
    {
        Task<bool> ExecuteAsync(TContext context);
    }
}
