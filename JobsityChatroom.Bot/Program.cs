using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

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
                    channel.QueueDeclare(queue: ConfigurationManager.AppSettings["RabbitMQCommandsQueue"],
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var requestMessage = Encoding.UTF8.GetString(body);
                        var responseMessage = "";

                        // Change the message to upper case, to avoid being case-sensitive
                        var command = requestMessage.Split('=')[0].ToUpper();
                        var param = requestMessage.Split('=')[1].ToUpper();

                        if (KnownCommands.KnowsCommand(command))
                        {
                            var task = (Task<string>)Type.GetType(KnownCommands.GetHandlerName(command)).GetMethod("Consume").Invoke(null, new object[] { param });
                            responseMessage = task.Result;
                        }
                        else
                        {
                            responseMessage = "I'm sorry but I can't understand that command, try with /stock";
                        }

                        // Send Message
                        using(var respChannel = connection.CreateModel())
                        {
                            var resBody = Encoding.UTF8.GetBytes(responseMessage);
                            channel.BasicPublish(exchange: "",
                                                 routingKey: ConfigurationManager.AppSettings["RabbitMQResponsesQueue"],
                                                 basicProperties: null,
                                                 body: resBody);
                        }

                    };
                    channel.BasicConsume(queue: ConfigurationManager.AppSettings["RabbitMQCommandsQueue"],
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
