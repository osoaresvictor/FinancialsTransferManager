using Newtonsoft.Json;
using System;
using TransactionsWorker.Infra;
using TransactionsWorker.Models;
using TransactionsWorker.Models.Enums;

namespace TransactionsWorker.Services
{
    public static class ProcessQueueHandler
    {
        public static readonly AcessoApiHandler _ApiAcessoResources = new AcessoApiHandler();
        public static readonly TransactionRegistersRepository _TransactionRegistersRepository = new TransactionRegistersRepository();

        public static void OnReceiveRequestInQueue(object sender, EventArgs e)
        {
            var transactionOrder = JsonConvert.DeserializeObject<Transaction>(sender.ToString());

            transactionOrder.Status.Status = TransactionStatus.Processing.ToString();
            _TransactionRegistersRepository.Update(transactionOrder.TransactionId, transactionOrder);

            (var originData, var destinationData) = BuildOriginAndDestinationData(transactionOrder);

            try
            {
                _ApiAcessoResources.MakeFinancialTransfer(originData, destinationData);

                transactionOrder.Status.Status = TransactionStatus.Confirmed.ToString();
                _TransactionRegistersRepository.Update(transactionOrder.TransactionId, transactionOrder);
            }
            catch (Exception ex)
            {
                transactionOrder.Status.Status = TransactionStatus.Error.ToString();
                transactionOrder.Status.Message = ex.Message;

                _TransactionRegistersRepository.Update(transactionOrder.TransactionId, transactionOrder);
            }
        }

        private static (BalanceAdjustment, BalanceAdjustment) BuildOriginAndDestinationData(Transaction transactionOrder)
        {
            var originData = new BalanceAdjustment
            {
                AccountNumber = transactionOrder.TransactionRequest.AccountOrigin,
                Type = TransactionType.Debit.ToString(),
                Value = transactionOrder.TransactionRequest.Value
            };

            var destinationData = new BalanceAdjustment
            {
                AccountNumber = transactionOrder.TransactionRequest.AccountDestination,
                Type = TransactionType.Credit.ToString(),
                Value = transactionOrder.TransactionRequest.Value
            };

            return (originData, destinationData);
        }
    }
}
