using Microsoft.Extensions.DependencyInjection;
using TelegramBot.ChatEngine.Commands.Routing;
using TelegramBot.ChatEngine.Implementation;

namespace TelegramBot.ChatEngine.Setup
{
    public class MessageHandlerBuilder
    {
        public IServiceCollection Services { get; }

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
            var result = new TelegramBotMessageHandler(serviceProvider, senderAction);
            return result;
        }
    }
}
