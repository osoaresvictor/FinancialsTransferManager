namespace TransactionsWorker.Models
{
    public class TransactionRequest
    {
        public string AccountOrigin { get; set; }
        public string AccountDestination { get; set; }
        public float Value { get; set; }
    }
}