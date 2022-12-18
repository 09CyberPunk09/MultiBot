using Microsoft.Extensions.Configuration;

namespace Common.Configuration;

public static class ConfigurationHelper
{
    public static IConfigurationRoot GetConfiguration()
    {
        var environment = Environment.GetEnvironmentVariable("ENV");
        var confBuilder = new ConfigurationBuilder();
        confBuilder.SetBasePath(AppContext.BaseDirectory);

        //load main appsettings file
        confBuilder.AddJsonFile($"appSettings.json");

        //load environment dependent configuration
        confBuilder.AddJsonFile($"appSettings.{environment}.json");
        return confBuilder.Build();
    }
}
