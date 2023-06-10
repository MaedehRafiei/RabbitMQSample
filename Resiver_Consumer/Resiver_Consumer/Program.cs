using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Resiver_Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            // because this queue is created in sender , it is not necessary to difine it 
            channel.QueueDeclare("myQueue1", false, false, false, null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArg) =>
            {
                var body = eventArg.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Random random = new Random();
                int sleep = random.Next(0, 3) * 1000;

                Console.WriteLine($"sleep:{sleep} delivery tags {eventArg.DeliveryTag}");
                Thread.Sleep(sleep);
                Console.WriteLine("Resived Message =>:" + message);
                //Thread.Sleep(1);

               // channel.BasicAck(eventArg.DeliveryTag, true);
            };

            channel.BasicConsume("myQueue1", false, consumer);
            channel.Close();
            connection.Close();

            Console.ReadLine();
        }
    }
}
