using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Infrastructure.Queuing
{
    public class QueuePublisher
    {
        private readonly string _hostName;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;

        public QueuePublisher(string hostName, string queueName, string username, string password)
        {
            _hostName = hostName;
            _queueName = queueName;
            _username = username;
            _password = password;
        }

        public void Publish(object objectToSend)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                UserName = _username,
                Password = _password
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            {
                channel.QueueDeclare(queue: _queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string objectJson = JsonConvert.SerializeObject(objectToSend);
                var body = Encoding.UTF8.GetBytes(objectJson);

                channel.BasicPublish(exchange: "",
                                     routingKey: _queueName,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}