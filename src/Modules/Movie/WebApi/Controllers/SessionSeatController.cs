using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Application.DTO;
using Movie.Domain.Interfaces.Services;

namespace Movie.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public class SessionSeatController : ControllerBase
    {
        private readonly ISessionSeatService _sessionSeatService;

        public SessionSeatController(ISessionSeatService sessionSeatService)
        {
            _sessionSeatService = sessionSeatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var seats = await _sessionSeatService.GetAllSessionSeats();
            return Ok(seats);
        }

        [HttpGet("{id:long}")]
        [Authorize]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var seat = await _sessionSeatService.GetSessionSeatById(id);
                return Ok(seat);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("session/{sessionId:long}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBySessionId(long sessionId)
        {
            var seats = await _sessionSeatService.GetSessionSeatsBySessionId(sessionId);
            return Ok(seats);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] SessionSeatDTO request)
        {
            await _sessionSeatService.AddSessionSeat(request);
            return Ok(new { message = "Session seat created" });
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(long id, [FromBody] SessionSeatUpdateDTO request)
        {
            request.Id = id;
            try
            {
                await _sessionSeatService.UpdateSessionSeat(request);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _sessionSeatService.DeleteSessionSeat(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
