using FinancialsTransfersManager.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace FinancialsTransfersManager.Infra.MongoDB
{
    public class TransactionRegistersRepository : ITransactionRegistersRepository
    {
        private readonly IMongoCollection<Transaction> _transaction;

        public TransactionRegistersRepository()
        {
            var mongoConnectionData = new { 
                MongoCollectionName = Environment.GetEnvironmentVariable("MongoCollectionName"), 
                MongoConnectionString = Environment.GetEnvironmentVariable("MongoConnectionString"),
                MongoDatabaseName = Environment.GetEnvironmentVariable("MongoDatabaseName")
            };


            var client = new MongoClient(mongoConnectionData.MongoConnectionString);
            var database = client.GetDatabase(mongoConnectionData.MongoDatabaseName);

            this._transaction = database.GetCollection<Transaction>(mongoConnectionData.MongoCollectionName);
        }

        public List<Transaction> Get() =>
            this._transaction.Find(transaction => true).ToList();

        public Transaction Get(string transactionId) =>
            this._transaction.Find(transaction => transaction.TransactionId == transactionId).FirstOrDefault();

        public Transaction Create(Transaction transaction)
        {
            this._transaction.InsertOne(transaction);

            return transaction;
        }

        public void Update(string transactionId, Transaction transactionIn) =>
            this._transaction.ReplaceOne(transaction => transaction.TransactionId == transactionId, transactionIn);

        public void Remove(Transaction transactionIn) =>
            this._transaction.DeleteOne(transaction => transaction.TransactionId == transactionIn.TransactionId);

        public void Remove(string transactionId) =>
            this._transaction.DeleteOne(transaction => transaction.TransactionId == transactionId);
    }
}
