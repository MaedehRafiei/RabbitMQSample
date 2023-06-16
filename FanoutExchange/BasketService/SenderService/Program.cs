using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace SenderService
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare("order.create", false, true, false);
            channel.QueueBind("order.create", "order", "");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, EventArgs) =>
            {
                var body = EventArgs.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Resive message " + message);
            };

            //bind consumer to queue
            channel.BasicConsume(queue: "order.create", true, consumer);
            Console.ReadLine();
        }
    }
}
