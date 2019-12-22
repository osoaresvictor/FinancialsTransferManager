namespace FinancialsTransfersManager.Infra.RabbitMQ
{
    public interface IRabbitMQProducerClient
    {
        void SendMessage(string queueName, string messageToSend);
    }
}