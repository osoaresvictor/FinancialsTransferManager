using MongoDB.Bson.Serialization.Attributes;

namespace TransactionsWorker.Models
{
    public class Transaction
    {
        [BsonId]
        public string TransactionId { get; set; }
        public TransactionRequest TransactionRequest { get; set; }
        public StatusResponse Status { get; set; }
    }
}
