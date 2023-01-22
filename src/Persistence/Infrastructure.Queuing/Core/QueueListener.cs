namespace Infrastructure.Queuing.Core;

public class QueueListener<TMessage>
{
    private protected string _queueName;
    public delegate void QueueMessageHandlerDelegate(TMessage response);
    public QueueListener(string queueName)
    {
        _queueName = queueName;
    }

    public virtual void AddMessageHandler(QueueMessageHandlerDelegate dg)
    {

    }
    public virtual void StartConsuming()
    {

    }
}