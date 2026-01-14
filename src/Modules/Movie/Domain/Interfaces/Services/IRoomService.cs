using System.Collections.Generic;
using System.Threading.Tasks;
using Movie.Application.DTO;
using Movie.Domain.Models.impl;

namespace Movie.Domain.Interfaces.Services
{
    internal interface IRoomService
    {
        Task<IEnumerable<RoomResponseDTO>> GetAllRooms();
        Task<RoomResponseDTO?> GetRoomById(long roomId);
        Task AddRoom(RoomDTO room);
        Task UpdateRoom(RoomUpdateDTO room);
        Task DeleteRoom(long roomId);
    }
}