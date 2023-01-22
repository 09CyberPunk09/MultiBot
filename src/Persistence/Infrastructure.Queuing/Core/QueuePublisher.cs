using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
