using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;
using log4net;

namespace RabbitMQEvaluation
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.BasicConfigurator.Configure();
            ILog log = log4net.LogManager.GetLogger(typeof(Program));

            log.Info("Bout to call TestMethod");
            TestMethod(log);
        }

        static void TestMethod(ILog logger)
        {
            //SendMessage(logger, "hello", "Hello World!");
            ReceiveFromQueue(logger, "hello");

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }

        private static void SendMessage(ILog logger, string queueName, string message)
        {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {

                            channel.QueueDeclare(queue: queueName,
                                             durable: false,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                            var body = Encoding.UTF8.GetBytes(message);

                            channel.BasicPublish(exchange: "",
                                routingKey: "hello",
                                basicProperties: null,
                                body: body);
                    }
                }
        }                

        private static string ReceiveFromQueue(ILog logger, string queueName)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    //The Rabbit MQ will actually push us the message. Therefore we need to declare a handler to accept
                    //the callback.
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        logger.Info("Message received: "+message);
                    };

                    channel.BasicConsume(queue: "hello",
                                         noAck: true,
                                         consumer: consumer);

                }
            }
            return null;
        }
    }
}
