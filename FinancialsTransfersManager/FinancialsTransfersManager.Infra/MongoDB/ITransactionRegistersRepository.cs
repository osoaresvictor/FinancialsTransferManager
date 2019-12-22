using FinancialsTransfersManager.Models;
using System.Collections.Generic;

namespace FinancialsTransfersManager.Infra.MongoDB
{
    public interface ITransactionRegistersRepository
    {
        Transaction Create(Transaction transaction);
        List<Transaction> Get();
        Transaction Get(string transactionId);
        void Remove(string transactionId);
        void Remove(Transaction transactionIn);
        void Update(string transactionId, Transaction transactionIn);
    }
}