using FinancialsTransfersManager.Infra.MongoDB;
using FinancialsTransfersManager.Models;
using FinancialsTransfersManager.Models.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialsTransfersManager.Services.Transactions.Tests
{
    [TestClass()]
    public class GetTransactionStatusTests
    {
        [TestMethod()]
        public void GetTransactionStatus_NoTransactionFound_ShouldReturnsNull()
        {
            var transactionId = "transactionId-123";
            var transactionRegistersRepositoryMock = new Mock<ITransactionRegistersRepository>();
            transactionRegistersRepositoryMock.Setup(repo => repo.Get(transactionId)).Returns(new Transaction());

            IGetTransactionStatus getTransactionStatus = new GetTransactionStatus(transactionRegistersRepositoryMock.Object);
            var result = getTransactionStatus.Execute(transactionId);

            Assert.IsNull(result);
        }

        [TestMethod()]
        public void GetTransactionStatus_TransactionFound_ShouldReturnsStatus()
        {
            var transactionId = "transactionId-456";
            var transactionFound = new Transaction { Status = new StatusResponse { Status = "Confirmed" } };
            var transactionRegistersRepositoryMock = new Mock<ITransactionRegistersRepository>();
            transactionRegistersRepositoryMock.Setup(repo => repo.Get(transactionId)).Returns(transactionFound);

            IGetTransactionStatus getTransactionStatus = new GetTransactionStatus(transactionRegistersRepositoryMock.Object);
            var result = getTransactionStatus.Execute(transactionId);

            Assert.AreEqual(transactionFound.Status, result);
        }
    }
}