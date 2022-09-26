using Microsoft.Extensions.Configuration;

namespace Kernel
{
    public static class ConfigurationHelper
    {
        public static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appSettings.json").Build();
        }
    }
}
