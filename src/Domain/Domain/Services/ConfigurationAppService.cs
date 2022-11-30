using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class ConfigurationAppService : AppService
{
    private IConfigurationRoot _configuration;

    public ConfigurationAppService()
    {
        var configurationBuilder = new ConfigurationBuilder()
         .AddJsonFile("appSettings.json");
        _configuration = configurationBuilder.Build();
    }
    public IConfigurationRoot GetConfigurationRoot()
    {
        return _configuration;
    }
    public string Get(string key)
    {
        return _configuration[key];
    }
}