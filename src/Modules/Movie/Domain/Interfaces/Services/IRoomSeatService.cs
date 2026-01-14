using System.Collections.Generic;
using System.Threading.Tasks;
using Movie.Application.DTO;
using Movie.Domain.Models.impl;

namespace Movie.Domain.Interfaces.Services
{
    internal interface IRoomSeatService
    {
        Task<IEnumerable<RoomSeatDTO>> GetAllRoomSeats();
        Task<RoomSeatDTO?> GetRoomSeatById(RoomSeatRequestDTO request);
        Task AddRoomSeat(RoomSeatDTO roomSeat);
        Task UpdateRoomSeat(RoomSeatUpdateDTO roomSeat);
        Task DeleteRoomSeat(long roomSeatId);
    }
}