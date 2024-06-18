using TelegramBot.ChatEngine.Commands.Middlewares;

namespace TelegramBot.ChatEngine.Setup.Middleware;

public class OperationMiddlewareBuilder
{
    public List<Type> MiddlewareTypes { get; } = [];

    /// <summary>
    /// Add a middleware in the middleware list.Note: do not forget to register the middleware in ServiceCollection
    /// </summary>
    /// <typeparam name="TMiddleware"></typeparam>
    public void Add<TMiddleware>() where TMiddleware : ITelegramMiddleware
    {
        MiddlewareTypes.Add(typeof(TMiddleware));
    }
    internal MiddlewareHandler Build(IServiceProvider provider)
    {
        return new MiddlewareHandler(provider, MiddlewareTypes);
    }
}
