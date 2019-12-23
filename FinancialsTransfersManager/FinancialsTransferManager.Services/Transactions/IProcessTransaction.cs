using FinancialsTransfersManager.Models;
using FinancialsTransfersManager.Models.DTOs;

namespace FinancialsTransfersManager.Services.Transactions
{
    public interface IProcessTransaction
    {
        Transaction Execute(TransactionRequest transactionRequest);
    }
}