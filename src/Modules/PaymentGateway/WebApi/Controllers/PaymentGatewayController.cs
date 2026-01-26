using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.DTO;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.WebApi.Controllers
{
    [ApiController]
    [Route("api/gateway")]
    public class PaymentGatewayController : ControllerBase
    {
        private readonly IPaymentGatewayService _gatewayService;

        public PaymentGatewayController(IPaymentGatewayService gatewayService)
        {
            _gatewayService = gatewayService;
        }

        [HttpPost("process")]
        public async Task<ActionResult<ProcessPaymentResponseDTO>> ProcessPayment([FromBody] ProcessPaymentRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _gatewayService.ProcessPaymentAsync(request);
            return Ok(result);
        }

        [HttpGet("transactions/{transactionId}")]
        public async Task<ActionResult<TransactionStatusDTO>> GetTransactionStatus(string transactionId)
        {
            var result = await _gatewayService.GetTransactionStatusAsync(transactionId);
            
            if (result == null)
                return NotFound(new { message = "Transaction not found" });

            return Ok(result);
        }

        [HttpGet("transactions")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<TransactionStatusDTO>>> GetAllTransactions()
        {
            var result = await _gatewayService.GetAllTransactionsAsync();
            return Ok(result);
        }

        [HttpPost("refund")]
        public async Task<ActionResult<RefundResponseDTO>> Refund([FromBody] RefundRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _gatewayService.RefundAsync(request);
            return Ok(result);
        }
    }
}
