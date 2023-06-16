using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace EmailService
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare("order.cancel", false, false , false);
            channel.QueueBind("order.cancel", "order", "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, EventArgs) =>
            {
                var body = EventArgs.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Resive message " + message);
            };

            //bind consumer to queue
            channel.BasicConsume(queue: "order.cancel", true, consumer);
            Console.ReadLine();
        }
    }
}
