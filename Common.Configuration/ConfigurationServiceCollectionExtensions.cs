using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Common.Configuration;

public static class ConfigurationServiceCollectionExtensions
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration);
        logger.Info("Added configuration");
        return services;
    }
    public static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        var configuration = ConfigurationHelper.GetConfiguration();
        services.AddSingleton(configuration);
        logger.Info("Added configuration");
        return services;
    }


    public static IServiceCollection AddSettings(this IServiceCollection services)
    {

        return services;
    }
}
