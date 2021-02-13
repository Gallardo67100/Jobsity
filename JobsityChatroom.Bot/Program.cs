using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Configuration;

namespace JobsityChatroom.Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Open the connection with the RabbitMQ Queue
                var factory = new ConnectionFactory() { HostName = ConfigurationManager.AppSettings["RabbitMQHost"] };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: ConfigurationManager.AppSettings["RabbitMQQueueName"],
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var stockCode = Encoding.UTF8.GetString(body);

                        var responseMessage = APIConsumer.GetStockInformation(stockCode).Result;

                        // Send Message
                        body = Encoding.UTF8.GetBytes(responseMessage);
                        channel.BasicPublish(exchange: "",
                                             routingKey: ConfigurationManager.AppSettings["RabbitMQQueueName"],
                                             basicProperties: null,
                                             body: body);

                    };
                    channel.BasicConsume(queue: ConfigurationManager.AppSettings["RabbitMQQueueName"],
                                         autoAck: true,
                                         consumer: consumer);

                    Console.ReadLine();
                }

                // Keeps the bot open, waiting for further messages
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
