using Common.Configuration;
using Infrastructure.Queuing.Core;
using Microsoft.Extensions.Configuration;
using NLog;

namespace Infrastructure.Queuing.RabbitMQ;

public class RabbitMQQueueingFactory : IQueuingFactory
{
    private static IConfiguration _configuration;
    public static Logger _logger = LogManager.GetCurrentClassLogger();
    public QueuePublisher CreatePublisher(string queueName)
    {
       if(_configuration == null)
        {
            _configuration = ConfigurationHelper.GetConfiguration();
        }

        var hostKey = IsDebugMode ? "RabbitMQ:Host" : "RabbitMQ:ContainerHost";
        // var hostKey = "RabbitMQ:Host";
        _logger.Info($"Debug Mode: {IsDebugMode}. RabbitMQ host: {_configuration[hostKey]}");
        var hostName = _configuration[hostKey];
        var username = _configuration["RabbitMQ:Username"];
        var password = _configuration["RabbitMQ:Password"];
        var port = _configuration["RabbitMQ:Port"];

        return new RabbitMQQueuePublisher(hostName, queueName, username, password, int.Parse(port));
    }
    public static bool IsDebugMode
    {
        get
        {
#if DEBUG
            return true;
#else
        return false;
#endif
        }
    }
    public QueueListener<T> CreateListenner<T>(string queueName)
    {
        if (_configuration == null)
        {
            _configuration = ConfigurationHelper.GetConfiguration();
        }
        var hostKey = IsDebugMode ? "RabbitMQ:Host" : "RabbitMQ:ContainerHost";
        // var hostKey = "RabbitMQ:Host";
        _logger.Info($"Debug Mode: {IsDebugMode}. RabbitMQ host: {_configuration[hostKey]}");

        var hostName = _configuration[hostKey];
        var username = _configuration["RabbitMQ:Username"];
        var password = _configuration["RabbitMQ:Password"];
        var port = _configuration["RabbitMQ:Port"];

        return new RabbitMQQueueListenner<T>(hostName, queueName, username, password, int.Parse(port));
    }
}
