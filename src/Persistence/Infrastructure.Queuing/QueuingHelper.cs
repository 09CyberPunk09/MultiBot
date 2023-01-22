using Common.Configuration;
using Infrastructure.Queuing.Core;
using Infrastructure.Queuing.RabbitMQ;
using Infrastructure.Queuing.Redis;
using NLog;

namespace Infrastructure.Queuing;

public static class QueuingHelper
{
    public static Logger _logger = LogManager.GetCurrentClassLogger();
    private static IQueuingFactory factory = new RabbitMQQueueingFactory();
    public static QueuePublisher CreatePublisher(string queueName)
    {
        return factory.CreatePublisher(queueName);
    }
    public static QueueListener<TQueryMessageType> CreateListener<TQueryMessageType>(string queueName)
    {
        return factory.CreateListenner<TQueryMessageType>(queueName);
    }
}
