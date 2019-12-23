
using FinancialsTransfersManager.Infra.MongoDB;
using FinancialsTransfersManager.Infra.RabbitMQ;
using FinancialsTransfersManager.Models;
using FinancialsTransfersManager.Models.DTOs;
using FinancialsTransfersManager.Models.Enums;
using Newtonsoft.Json;
using System;

namespace FinancialsTransfersManager.Services.Transactions
{
    public class ProcessTransaction : IProcessTransaction
    {
        private readonly ITransactionRegistersRepository _ITransactionRegistersRepository;
        private readonly IRabbitMQProducerClient _IRabbitMQProducerClient;

        public ProcessTransaction(ITransactionRegistersRepository ITransactionRegistersRepository,
                                                   IRabbitMQProducerClient IRabbitMQProducerClient)
        {
            this._ITransactionRegistersRepository = ITransactionRegistersRepository;
            this._IRabbitMQProducerClient = IRabbitMQProducerClient;
        }

        public Transaction Execute(TransactionRequest transactionRequest)
        {
            var transaction = new Transaction
            {
                TransactionRequest = transactionRequest,
                TransactionId = Guid.NewGuid().ToString(),
                Status = new StatusResponse
                {
                    Status = TransactionStatus.In_Queue.ToString(),
                    Message = ""
                }
            };

            this._ITransactionRegistersRepository.Create(transaction);

            var message = JsonConvert.SerializeObject(transaction);
            var queueName = Environment.GetEnvironmentVariable("QueueName");

            this._IRabbitMQProducerClient.SendMessage(queueName, message);

            return transaction;
        }

    }
}
