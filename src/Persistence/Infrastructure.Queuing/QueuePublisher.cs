using Newtonsoft.Json;
using ServiceStack.Redis;

namespace Infrastructure.Queuing
{
    public class QueuePublisher
    {
        private readonly RedisClient _client;
        private readonly string _queueName;
        public QueuePublisher(string hostName, int port, string queueName)
        {
            _client = new(hostName, port);
            _queueName = queueName;
        }

        public void Publish(object objectToSend)
        {
            var message = JsonConvert.SerializeObject(objectToSend);
            _client.PublishMessage(_queueName, message);
        }
    }
}