namespace TelegramBot.ChatEngine.Commands.Middlewares;

public class MiddlewareHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly List<Type> _middlewareTypes = [];
    public MiddlewareHandler(IServiceProvider serviceProvider, List<Type> middlewareTypes)
    {
        _middlewareTypes = middlewareTypes;
        _serviceProvider = serviceProvider;
    }

    public async Task<bool> ExecuteMiddlewares(TelegramMessageContext context)
    {
        foreach (var middlewareType in _middlewareTypes)
        {
            var resolvedMiddleware = _serviceProvider.GetService(middlewareType) as ITelegramMiddleware
                                                                                ?? throw new NullReferenceException($"Type {middlewareType.FullName} was not registered into service provider.");
            bool succeeded = await resolvedMiddleware.ExecuteAsync(context);
            if (!succeeded) return false;
        }
        return true;
    }
}
