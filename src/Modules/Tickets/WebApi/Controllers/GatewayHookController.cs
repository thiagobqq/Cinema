using Microsoft.AspNetCore.Mvc;
using Tickets.Application.DTO;
using Tickets.Domain.Interfaces.Services;

namespace Tickets.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GatewayHookController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private const string API_KEY = "123456";

        public GatewayHookController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> GatewayHook([FromBody] GatewayWebhookDTO payload, [FromQuery] string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey) || apiKey != API_KEY)
            {
                return Unauthorized(new { message = "Invalid API key" });           
            }

            await _paymentService.ProcessWebhookAsync(payload);
            return Ok();
        }
    }
}