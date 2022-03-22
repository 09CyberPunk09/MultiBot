using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Infrastructure.Queuing
{
    public class QueueListener<TQueryMessageType>
    {
        private readonly string _hostName;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;
        public delegate void QueueMessageHandlerDelegate(TQueryMessageType response);
        private event EventHandler<BasicDeliverEventArgs> Recieved;

        private readonly IConnection _connection;
        private IModel _channel;

        public QueueListener(string hostName, string queueName, string username, string password)
        {
            _hostName = hostName;
            _queueName = queueName;
            _username = username;
            _password = password;

            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Password = _password,
                UserName = _username,
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
        }

        public void AddMessageHandler(QueueMessageHandlerDelegate dg)
        {
            Recieved += (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var response = JsonConvert.DeserializeObject<TQueryMessageType>(message);
                dg?.Invoke(response);
            };
        }

        public void StartConsuming()
        {
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += Recieved;
                _channel.BasicConsume(queue: _queueName,
                                     autoAck: true,
                                     consumer: consumer);
        }
    }
}
