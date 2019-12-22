using System;
using TransactionsWorker.Infra;

namespace TransactionsWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var queueName = Environment.GetEnvironmentVariable("QueueName");

            Console.WriteLine($"{DateTime.Now} Start queue listening...");

            var queueListener = new RabbitMQConsumerClient();
            queueListener.EvtReceive += Services.ProcessQueueHandler.OnReceiveRequestInQueue;
            queueListener.ReceiveMessage(queueName);

            Console.WriteLine("Stop listening...");
        }
    }
}
