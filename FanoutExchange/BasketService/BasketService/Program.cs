using RabbitMQ.Client;
using System;
using System.Text;

namespace BasketService
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("order", ExchangeType.Fanout, false);   

            string message = $"send shopping cart";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "order", routingKey: "", basicProperties: null, body);
        }
    }
}
