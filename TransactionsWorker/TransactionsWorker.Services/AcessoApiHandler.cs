using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading;
using TransactionsWorker.Models;
using TransactionsWorker.Models.Enums;

namespace TransactionsWorker.Infra
{
    public class AcessoApiHandler
    {
        public readonly string BASE_ENDPOINT = Environment.GetEnvironmentVariable("AcessoApi");

        public void MakeFinancialTransfer(BalanceAdjustment originData, BalanceAdjustment destinationData)
        {
            var originResponse = this.DebitMoney(originData);
            Console.WriteLine(originResponse?.ErrorMessage);
            if (originResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Service Unavailable, try again later | {originResponse?.Content}");
            }

            var destinationResponse = this.CreditMoney(destinationData);
            if (destinationResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                this.RollBackTransactionAndAbortProcess(originData, destinationResponse.Content);
            }

        }

        private IRestResponse DebitMoney(BalanceAdjustment dataToDebitMoney)
        {
            dataToDebitMoney.Type = TransactionType.Debit.ToString();

            var postAccountEndpoint = $"{this.BASE_ENDPOINT}/api/Account";
            var client = new RestClient(postAccountEndpoint);

            var requestOrigin = new RestRequest(Method.POST);
            requestOrigin.AddHeader("content-type", "application/json");
            requestOrigin.AddParameter(
                "application/json",
                JsonConvert.SerializeObject(dataToDebitMoney),
                ParameterType.RequestBody
            );

            var responseOrigin = client.Execute(requestOrigin);
            if (responseOrigin.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                responseOrigin = this.RetryRequest(client, requestOrigin, 5);
            }

            return responseOrigin;
        }

        private IRestResponse CreditMoney(BalanceAdjustment dataToCreditMoney)
        {
            dataToCreditMoney.Type = TransactionType.Credit.ToString();

            var postAccountEndpoint = $"{this.BASE_ENDPOINT}/api/Account";
            var client = new RestClient(postAccountEndpoint);

            var requestDestination = new RestRequest(Method.POST);
            requestDestination.AddHeader("content-type", "application/json");
            requestDestination.AddParameter(
                "application/json",
                JsonConvert.SerializeObject(dataToCreditMoney),
                ParameterType.RequestBody
            );
            var responseDestination = client.Execute(requestDestination);

            if (responseDestination.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                responseDestination = this.RetryRequest(client, requestDestination, 5);
            }

            return responseDestination;
        }

        private IRestResponse RetryRequest(RestClient client, RestRequest requestOrigin, int retries)
        {
            var tryNumber = 0;
            var response = default(IRestResponse);
            while (
                (response == default(IRestResponse) || response.StatusCode != System.Net.HttpStatusCode.OK)
                && tryNumber < retries
            )
            {
                retries++;
                Thread.Sleep(800);
                response = client.Execute(requestOrigin);
            }

            return response;
        }

        private void RollBackTransactionAndAbortProcess(BalanceAdjustment originData, string error)
        {
            this.CreditMoney(originData);
            throw new Exception($"Transaction Cancelled: {error}");
        }
    }
}
