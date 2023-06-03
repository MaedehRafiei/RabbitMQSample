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
            var chanel = connection.CreateModel();
            // because this queue is created in sender , it is not necessary to difine it 
            // chanel.QueueDeclare("myQueue1", false, false, false, null);
            var consumer = new EventingBasicConsumer(chanel);
            consumer.Received += (model, eventArg) =>
            {
                var body = eventArg.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Resived Message =>:" + message);
                Thread.Sleep(1);
            };

            chanel.BasicConsume("myQueue1", true, consumer);
            chanel.Close();
            connection.Close();

            Console.ReadLine();
        }
    }
}
