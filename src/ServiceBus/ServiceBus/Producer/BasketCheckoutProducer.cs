using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using ServiceBus.Events;

namespace ServiceBus.Producer
{
    public class BasketCheckoutProducer
    {
        private readonly IRabbitMqConnection _connection;

        public BasketCheckoutProducer(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public void PublishBasketCheckout(string queueName, BasketCheckoutEvent publishModel)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false,
                    arguments: null);
                var message = JsonConvert.SerializeObject(publishModel);
                var body = Encoding.UTF8.GetBytes(message);

                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;
                
                channel.ConfirmSelect();
                channel.BasicPublish(exchange: string.Empty, routingKey: queueName, mandatory: true,
                    basicProperties: properties, body: body
                );
                channel.WaitForConfirmsOrDie();
                
                channel.BasicAcks += (sender, eventArgs) =>
                {
                    Console.WriteLine($"Sent to the queue : {queueName}");
                };
                channel.ConfirmSelect();
            }
        }
    }
}