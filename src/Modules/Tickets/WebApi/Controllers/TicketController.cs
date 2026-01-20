using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickets.Application.DTO;
using Tickets.Domain.Interfaces.Services;

namespace Tickets.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService) => _ticketService = ticketService;
        

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BuyTicket([FromBody] BuyTicketDTO buyTicketDTO)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userid == null)
                return Unauthorized();
            var result = await _ticketService.buyTicket(buyTicketDTO, userid);
            return Ok(result);
            
        }


    }
}