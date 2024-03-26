using Application.Chatting.Core.Context;
using System.Threading.Tasks;

namespace Application.Chatting.Core.Middlewares.Interfaces
{
    public interface IMiddleware<TContext> where TContext : MessageContext
    {
        Task<bool> ExecuteAsync(TContext context);
    }
}
