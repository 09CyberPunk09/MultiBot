using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class ConfigurationAppService : AppService
{
    private IConfiguration _configuration;

    public ConfigurationAppService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public IConfiguration GetConfigurationRoot()
    {
        return _configuration;
    }
    public string Get(string key)
    {
        return _configuration[key];
    }
}