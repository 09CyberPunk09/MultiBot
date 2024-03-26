using Microsoft.Extensions.Configuration;

namespace Common.Configuration;

public static class ConfigurationHelper
{
    private static IConfigurationRoot _instance;
    public static IConfigurationRoot GetConfiguration()
    {
        if (_instance == null)
        {
            var environment = Environment.GetEnvironmentVariable("ENV");
            var confBuilder = new ConfigurationBuilder();
            confBuilder.SetBasePath(AppContext.BaseDirectory);

            //load main appsettings file
            confBuilder.AddJsonFile($"appSettings.json");

            //load environment dependent configuration
            confBuilder.AddJsonFile($"appSettings.{environment}.json");
            _instance = confBuilder.Build();
            return _instance;
        }
        else
        {
            return _instance;
        }
    }
}
