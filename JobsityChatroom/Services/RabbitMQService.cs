using JobsityChatroom.Properties;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsityChatroom.Services
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly ConnectionFactory factory;
        private readonly ChatroomHub hub;
        private readonly RabbitMQSettings settings;
        private readonly IConnection commandConnection;
        private readonly IModel commandChannel;
        private readonly IConnection responseConnection;
        private readonly IModel responseChannel;

        public RabbitMQService(ConnectionFactory factory, ChatroomHub hub, RabbitMQSettings settings)
        {
            this.factory = factory;
            this.hub = hub;
            this.settings = settings;
            this.commandConnection = factory.CreateConnection();
            this.commandChannel = this.commandConnection.CreateModel();
            this.responseConnection = factory.CreateConnection();
            this.responseChannel = this.responseConnection.CreateModel();

            commandChannel.QueueDeclare(queue: this.settings.CommandQueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            responseChannel.QueueDeclare(queue: this.settings.ResponseQueueName,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

            var consumer = new EventingBasicConsumer(responseChannel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var responseMessage = Encoding.UTF8.GetString(body);

                this.ReceiveFromQueue(responseMessage).Wait();
            };
            responseChannel.BasicConsume(queue: this.settings.ResponseQueueName,
                                 autoAck: true,
                                 consumer: consumer);


        }

        public void SendToQueue(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            commandChannel.BasicPublish(exchange: "",
                                 routingKey: this.settings.CommandQueueName,
                                 basicProperties: null,
                                 body: body);
        }

        public async Task ReceiveFromQueue(string message)
        {
            await hub.SendMessage("Chatbot", message, DateTime.Now);
        }
    }
}
