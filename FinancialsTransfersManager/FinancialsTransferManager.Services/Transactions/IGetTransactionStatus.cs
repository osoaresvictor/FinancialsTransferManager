using FinancialsTransfersManager.Models.DTOs;

namespace FinancialsTransfersManager.Services.Transactions
{
    public interface IGetTransactionStatus
    {
        StatusResponse Execute(string transactionId);
    }
}