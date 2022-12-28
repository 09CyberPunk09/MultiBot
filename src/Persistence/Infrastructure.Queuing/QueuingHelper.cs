using Common.Configuration;
using NLog;

namespace Infrastructure.Queuing
{
    public static class QueuingHelper
    {
        public static Logger _logger = LogManager.GetCurrentClassLogger();
        public static QueuePublisher CreatePublisher(string queueName)
        {
            var configuration = ConfigurationHelper.GetConfiguration();

            var hostName = configuration["Redis:Default:HostName"];
            var port = int.Parse(configuration["Redis:Default:Port"]);

            return new(hostName, port, queueName);
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
            var configuration = ConfigurationHelper.GetConfiguration();

            var hostName = configuration["Redis:Default:HostName"];
            var port = int.Parse(configuration["Redis:Default:Port"]);

            return new(hostName, port, queueName);
        }
    }
}
