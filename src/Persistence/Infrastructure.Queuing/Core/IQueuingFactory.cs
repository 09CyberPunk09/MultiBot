namespace Infrastructure.Queuing.Core
{
    public interface IQueuingFactory
    {
        QueueListener<T> CreateListenner<T>(string queueName);
        QueuePublisher CreatePublisher(string queueName);
    }
}
