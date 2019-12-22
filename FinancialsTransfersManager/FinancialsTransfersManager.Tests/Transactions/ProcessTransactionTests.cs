using FinancialsTransfersManager.Infra.MongoDB;
using FinancialsTransfersManager.Infra.RabbitMQ;
using FinancialsTransfersManager.Models;
using FinancialsTransfersManager.Models.DTOs;
using FinancialsTransfersManager.Models.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace FinancialsTransfersManager.Services.Transactions.Tests
{
    [TestClass()]
    public class ProcessTransactionTests
    {
        [TestMethod()]
        public void ProcessTransaction_GivenThatTransactionRequestIsValid_ShouldCreateTransactionSendMessageAndReturnTransaction()
        {
            Environment.SetEnvironmentVariable("QueueName", "teste123");
            var transactionRequest = new TransactionRequest
            {
                AccountOrigin = "123",
                AccountDestination = "456",
                Value = 55.32F
            };
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
            var transactionRegistersRepositoryMock = new Mock<ITransactionRegistersRepository>();
            var rabbitMQProducerClientMock = new Mock<IRabbitMQProducerClient>();

            IProcessTransaction processTransaction = new ProcessTransaction(
                transactionRegistersRepositoryMock.Object, rabbitMQProducerClientMock.Object
            );
            var result = processTransaction.Execute(transactionRequest);

            transactionRegistersRepositoryMock.Verify(repoMock => repoMock.Create(It.IsAny<Transaction>()), Times.Once);
            rabbitMQProducerClientMock.Verify(queueMock => queueMock.SendMessage("teste123", It.IsAny<string>()), Times.Once);
            Assert.AreEqual(result.Status.Message, transaction.Status.Message);
            Assert.AreEqual(result.Status.Status, transaction.Status.Status);
            Assert.AreEqual(result.TransactionRequest, transaction.TransactionRequest);
        }
    }
}