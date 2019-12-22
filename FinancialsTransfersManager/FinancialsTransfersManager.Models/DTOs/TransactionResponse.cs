namespace FinancialsTransfersManager.Models.DTOs
{
    public class TransactionResponse
    {
        public string TransactionId { get; set; }

        public TransactionResponse(string TransactionId)
        {
            this.TransactionId = TransactionId;
        }
    }
}
