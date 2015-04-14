using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ.Receiver
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Wait for messages");
            WaitForMessages();
        }

        public static void WaitForMessages()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume("message", true, consumer);

                while (true)
                {
                    var eventArgs = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    var body = eventArgs.Body;
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine("Received: {0}", message);
                }
            }
        }
    }
}
