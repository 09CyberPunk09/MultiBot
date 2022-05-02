using Microsoft.Extensions.Configuration;
using NLog;
using System;

namespace Infrastructure.Queuing
{
    public static class QueuingHelper
    {
        public static Logger _logger = LogManager.GetCurrentClassLogger();
        public static QueuePublisher CreatePublisher(string queueName)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json").Build();

            var hostKey = IsDebugMode ? "RabbitMQ:Host" : "RabbitMQ:ContainerHost";
           // var hostKey = "RabbitMQ:Host";
            _logger.Info($"Debug Mode: {IsDebugMode}. RabbitMQ host: {configuration[hostKey]}");
            var hostName = configuration[hostKey];
            var username = configuration["RabbitMQ:Username"];
            var password = configuration["RabbitMQ:Password"];
            var port = configuration["RabbitMQ:Port"];

            return new(hostName, queueName, username, password,Int32.Parse(port));
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
        public static QueueListener<TQueryMessageType> CreateListener<TQueryMessageType>(string queueName)
        {
            var configuration = new ConfigurationBuilder()
              .AddJsonFile("appSettings.json").Build();

             var hostKey = IsDebugMode ? "RabbitMQ:Host" : "RabbitMQ:ContainerHost";
           // var hostKey = "RabbitMQ:Host";
            _logger.Info($"Debug Mode: {IsDebugMode}. RabbitMQ host: {configuration[hostKey]}");

            var hostName = configuration[hostKey];
            var username = configuration["RabbitMQ:Username"];
            var password = configuration["RabbitMQ:Password"];
            var port = configuration["RabbitMQ:Port"];

            return new(hostName, queueName, username, password,Int32.Parse(port));
        }
    }
}
