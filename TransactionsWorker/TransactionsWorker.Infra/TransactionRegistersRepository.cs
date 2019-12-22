using MongoDB.Driver;
using System;
using TransactionsWorker.Models;

namespace TransactionsWorker.Infra
{
    public class TransactionRegistersRepository
    {
        private readonly IMongoCollection<Transaction> _transaction;

        public TransactionRegistersRepository()
        {
            var mongoConnectionData = new
            {
                MongoCollectionName = Environment.GetEnvironmentVariable("MongoCollectionName"),
                MongoConnectionString = Environment.GetEnvironmentVariable("MongoConnectionString"),
                MongoDatabaseName = Environment.GetEnvironmentVariable("MongoDatabaseName")
            };

            var client = new MongoClient(mongoConnectionData.MongoConnectionString);
            var database = client.GetDatabase(mongoConnectionData.MongoDatabaseName);

            this._transaction = database.GetCollection<Transaction>(mongoConnectionData.MongoCollectionName);
        }

        public void Update(string transactionId, Transaction transactionIn) =>
            this._transaction.ReplaceOne(transaction => transaction.TransactionId == transactionId, transactionIn);
    }
}
