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
    [Authorize(Roles = "Admin")]
    public class RoomSeatController : ControllerBase
    {
        private readonly IRoomSeatService _roomSeatService;

        public RoomSeatController(IRoomSeatService roomSeatService)
        {
            _roomSeatService = roomSeatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var seats = await _roomSeatService.GetAllRoomSeats();
            return Ok(seats);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var seat = await _roomSeatService.GetRoomSeatById(new RoomSeatRequestDTO { Id = id });
                return Ok(seat);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomSeatDTO request)
        {
            await _roomSeatService.AddRoomSeat(request);
            return Ok(new { message = "Room seat created" });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] RoomSeatUpdateDTO request)
        {
            request.Id = id;
            try
            {
                await _roomSeatService.UpdateRoomSeat(request);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _roomSeatService.DeleteRoomSeat(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
