using System;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ.Sender
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Publishing Message...");
            Publish(args.Length > 0 ? String.Join(" ", args) : "Hello world!");
        }

        public static void Publish(string message)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("message", "direct", true);
                channel.QueueDeclare("message", true, false, false, null);
                channel.QueueBind("message", "message", "info");

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("message", "info", null, body);
            }
        }
    }
}
