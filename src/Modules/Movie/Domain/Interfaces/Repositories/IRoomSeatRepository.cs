using System.Collections.Generic;
using System.Threading.Tasks;
using Movie.Domain.Models.impl;

namespace Movie.Domain.Interfaces.Repositories
{
    internal interface IRoomSeatRepository
    {
        Task<IEnumerable<RoomSeat>> GetAllRoomSeats();
        Task<RoomSeat?> GetRoomSeatById(long roomSeatId);
        Task AddRoomSeat(RoomSeat roomSeat);
        Task UpdateRoomSeat(RoomSeat roomSeat);
        Task DeleteRoomSeat(long roomSeatId);
    }
}