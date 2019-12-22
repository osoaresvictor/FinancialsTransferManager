using FinancialsTransfersManager.Models.DTOs;
using MongoDB.Bson.Serialization.Attributes;

namespace FinancialsTransfersManager.Models
{
    public class Transaction
    {
        [BsonId]
        public string TransactionId { get; set; }
        public TransactionRequest TransactionRequest { get; set; }
        public StatusResponse Status { get; set; }
    }
}
