using Newtonsoft.Json;
using ServiceStack.Redis;
using System.Collections.Generic;

namespace Infrastructure.Queuing
{
    public class QueueListener<TQueryMessageType>
    {
        public delegate void QueueMessageHandlerDelegate(TQueryMessageType response);
        private List<QueueMessageHandlerDelegate> _subscriptions = new();
        private readonly RedisClient _client;
        private readonly IRedisSubscription _subscriptionAccessor;
        private readonly string _queueName;
        public QueueListener(string hostName, int port, string queueName)
        {
            _client = new(hostName, port);
            _subscriptionAccessor = _client.CreateSubscription();
            _queueName = queueName;
        }

        public void AddMessageHandler(QueueMessageHandlerDelegate dg)
        {
            _subscriptions.Add(dg);
        }

        public void StartConsuming()
        {
            _subscriptions.ForEach(x =>
            {
                _subscriptionAccessor.OnMessage += (channel, msg) =>
                {
                    if (channel == _queueName)
                    {
                        var response = JsonConvert.DeserializeObject<TQueryMessageType>(msg);
                        x?.Invoke(response);
                    }
                };
            });

            _subscriptionAccessor.SubscribeToChannels(_queueName);
        }
    }
}