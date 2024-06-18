using Microsoft.Extensions.DependencyInjection;
using TelegramBot.ChatEngine.Core;

namespace TelegramBot.ChatEngine.Setup;

public class CachingBuilder
{
    private readonly IServiceCollection _services;
    internal CachingBuilder(IServiceCollection services)
    {
        _services = services;
    }

    internal void EnsureCacheRegistered()
    {
        var customCacheRegistered = _services.Any(x => x.ServiceType == typeof(IBotCache));
        if (!customCacheRegistered)
        {
            RegisterDefaultCacheImplementation();
        }

    }

    private void RegisterDefaultCacheImplementation()
    {
        _services.AddTransient<IBotCache, DefaultBotCache>();
    }
}