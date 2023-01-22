using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queuing.Core
{
    public interface IQueuingFactory
    {
        QueueListener<T> CreateListenner<T>(string queueName);
        QueuePublisher CreatePublisher(string queueName);
    }
}
