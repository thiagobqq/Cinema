using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movie.Domain.Models.impl;

namespace Movie.Domain.Interfaces.Repositories
{
    internal interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllRooms();
        Task<Room?> GetRoomById(long roomId);
        Task AddRoom(Room room);
        Task UpdateRoom(Room room);
        Task DeleteRoom(long roomId);
        
    }
}