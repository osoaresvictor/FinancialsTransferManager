using RabbitMQ.Client;
using System;
using System.Text;

namespace FinancialsTransfersManager.Infra.RabbitMQ
{
    public class RabbitMQProducerClient : IRabbitMQProducerClient
    {
        public readonly string rabbitMQHost = Environment.GetEnvironmentVariable("RabbitMQHost");
        public readonly int rabbitMQPort = int.Parse(Environment.GetEnvironmentVariable("RabbitMQPort"));

        public void SendMessage(string queueName, string messageToSend)
        {
            var factory = new ConnectionFactory() { Endpoint = new AmqpTcpEndpoint(rabbitMQHost, rabbitMQPort) };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var body = Encoding.UTF8.GetBytes(messageToSend);

            channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine($" {DateTime.Now} - Sent Message: {messageToSend}");
        }
    }
}
