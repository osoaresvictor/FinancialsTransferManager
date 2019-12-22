using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Text;
using System.Threading;

namespace TransactionsWorker.Infra
{
    public class RabbitMQConsumerClient
    {
        public readonly string RABBITMQHOST = Environment.GetEnvironmentVariable("RabbitMQHost");
        public readonly int RABBITMQPORT = int.Parse(Environment.GetEnvironmentVariable("RabbitMQPort"));

        public event EventHandler EvtReceive;

        public void ReceiveMessage(string queueName)
        {
            var conncetionFactory = new ConnectionFactory() { Endpoint = new AmqpTcpEndpoint(this.RABBITMQHOST, this.RABBITMQPORT) };
            var connection = default(IConnection);

            while (connection == null)
            {
                try
                {
                    Console.WriteLine($"Connecting to {this.RABBITMQHOST}:{this.RABBITMQPORT}");
                    connection = conncetionFactory.CreateConnection();
                }
                catch (BrokerUnreachableException ex)
                {
                    Console.WriteLine($"Error: {ex.Message} - Reconnect in 5s");
                    Thread.Sleep(5000);
                }
            }

            Console.WriteLine($"Connection established!");

            using (var autoResetEvent = new AutoResetEvent(false))
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, eventArgs) =>
                {
                    var body = eventArgs.Body;
                    var receivedMessage = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" {DateTime.Now} - Received: {receivedMessage}");

                    this.EvtReceive.Invoke(receivedMessage, EventArgs.Empty);
                };

                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                autoResetEvent.WaitOne();
            }

            connection.Dispose();
        }
    }
}
