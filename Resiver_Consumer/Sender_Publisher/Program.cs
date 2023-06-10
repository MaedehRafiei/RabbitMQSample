using System;
using System.Text;
using RabbitMQ.Client;
namespace Sender_Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            var connection = factory.CreateConnection();
            var chanel = connection.CreateModel();
            chanel.QueueDeclare("myQueue1", true, false, false, null);
            for (int i = 0; i < 100; i++)
            {
                string message = $"this is a test message :{DateTime.Now.Ticks}";
                var body = Encoding.UTF8.GetBytes(message);
                var properties = chanel.CreateBasicProperties();
                properties.Persistent = true;
                chanel.BasicPublish("", "myQueue1", properties, body);
            }
            chanel.Close();
            connection.Close();

            Console.ReadLine();
        }
    }
}
