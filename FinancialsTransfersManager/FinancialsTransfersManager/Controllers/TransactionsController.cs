using FinancialsTransfersManager.Models.DTOs;
using FinancialsTransfersManager.Services.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialsTransfersManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IProcessTransaction _IProcessTransaction;
        private readonly IGetTransactionStatus _IGetStatus;

        public TransactionsController(IProcessTransaction IProcessTransaction, IGetTransactionStatus IGetStatus)
        {
            this._IProcessTransaction = IProcessTransaction;
            this._IGetStatus = IGetStatus;
        }

        [HttpPost]
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK), Produces("application/json")]
        public ActionResult<TransactionResponse> PostTransaction([FromBody] TransactionRequest transactionRequest)
        {
            if (!this.ModelState.IsValid) { return this.BadRequest("Invalid Request"); }
            
            var transaction = this._IProcessTransaction.Execute(transactionRequest);

            return new TransactionResponse(transaction.TransactionId);
        }

        [HttpPost, Route("Status")]
        [ProducesResponseType(typeof(StatusResponse), StatusCodes.Status200OK), Produces("application/json")]
        public ActionResult<StatusResponse> GetTransactionStatus([FromBody]  StatusRequest statusRequest)
        {
            var result = this._IGetStatus.Execute(statusRequest.TransactionId);

            if (result == null)
            {
                return NotFound($"Transaction '{statusRequest.TransactionId}' not found");
            }

            return Ok(result);
        }

    }
}
