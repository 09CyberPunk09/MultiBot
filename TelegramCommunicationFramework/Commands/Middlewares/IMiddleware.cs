namespace TelegramBot.ChatEngine.Commands.Middlewares;

public interface IMiddleware<TContext> where TContext : TelegramMessageContext
{
    Task<bool> ExecuteAsync(TContext context);
}
