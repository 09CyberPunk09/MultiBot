namespace Infrastructure.Queuing.Core;

public class QueuePublisher
{
    private protected string _queueName;
    public QueuePublisher(string queueName)
    {
        _queueName = queueName;
    }
    public virtual void Publish(object objectToSend)
    {

    }
}
