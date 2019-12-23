using FinancialsTransfersManager.Infra.MongoDB;
using FinancialsTransfersManager.Models.DTOs;

namespace FinancialsTransfersManager.Services.Transactions
{
    public class GetTransactionStatus : IGetTransactionStatus
    {
        private readonly ITransactionRegistersRepository _ITransactionRegistersRepository;

        public GetTransactionStatus(ITransactionRegistersRepository ITransactionRegistersRepository)
        {
            this._ITransactionRegistersRepository = ITransactionRegistersRepository;
        }

        public StatusResponse Execute(string transactionId)
        {
            var transaction = this._ITransactionRegistersRepository.Get(transactionId);
            return transaction?.Status;
        }
    }
}
