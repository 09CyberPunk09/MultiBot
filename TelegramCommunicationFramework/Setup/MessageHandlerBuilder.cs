using Microsoft.Extensions.DependencyInjection;
using TelegramBot.ChatEngine.Implementation;
using TelegramBot.ChatEngine.Setup.Middleware;

namespace TelegramBot.ChatEngine.Setup;

public class MessageHandlerBuilder
{
    public IServiceCollection Services { get; }
    public MiddlewareBuilder Middleware { get; set; }
    public MessageTransportationBuilder MessageTransportation { get; }
    public CachingBuilder Caching { get; }
    public LoggingBuilder Logging { get; }

    public MessageHandlerBuilder()
    {
        Services = new ServiceCollection();
        MessageTransportation = new ();
        Caching = new(Services);
        Logging = new();
    }

    public TelegramBotMessageHandler Build()
    {
        Caching.EnsureCacheRegistered();
        var serviceProvider = Services.BuildServiceProvider();

        var senderAction = MessageTransportation.GetSenderAction(serviceProvider);

        var poerCommandMiddlewares = Middleware.PerCommandMiddleware.Build(serviceProvider);
        var poerMessageMiddlewares = Middleware.PerMessageMiddleware.Build(serviceProvider);

        var result = new TelegramBotMessageHandler(serviceProvider, senderAction, poerCommandMiddlewares, poerMessageMiddlewares);
        return result;
    }
}
