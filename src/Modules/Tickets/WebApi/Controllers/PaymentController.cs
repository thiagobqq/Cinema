using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tickets.Domain.Interfaces.Services;

namespace Tickets.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("listAll")]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPayments();
            return Ok(payments);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetPaymentById(long id)
        {
            var payment = await _paymentService.GetPaymentById(id);
            if (payment == null)
                return NotFound();
            return Ok(payment);
        }

        [HttpGet("ticket/{ticketId:long}")]
        public async Task<IActionResult> GetPaymentsByTicketId(long ticketId)
        {
            var payments = await _paymentService.GetPaymentsByTicketId(ticketId);
            return Ok(payments);
        }
    }
}