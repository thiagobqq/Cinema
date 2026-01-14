using System.Collections.Generic;
using System.Threading.Tasks;
using Movie.Application.DTO;
using Movie.Domain.Interfaces.Repositories;
using Movie.Domain.Interfaces.Services;
using Movie.Domain.Models.impl;

namespace Movie.Application.Services
{
    internal class RoomService : IRoomService
    {
        private readonly IRoomRepository _repo;

        public RoomService(IRoomRepository repo) => _repo = repo;
        public async Task<IEnumerable<RoomResponseDTO>> GetAllRooms() {
            var rooms =  await _repo.GetAllRooms();
            return rooms.Select(r => new RoomResponseDTO
            {
                Id = r.Id,
                Name = r.Name,
                VenueId = r.VenueId
            });
        }

        public async Task<RoomResponseDTO?> GetRoomById(long roomId)
        {
            var room = await _repo.GetRoomById(roomId);

            if (room == null) 
                throw new KeyNotFoundException("Room not found");

            return new RoomResponseDTO
            {
                Id = room.Id,
                Name = room.Name,
                VenueId = room.VenueId
            };
        }

        public Task AddRoom(RoomDTO request)
        {
            var room = new Room
            {
                Name = request.Name,
                VenueId = request.VenueId
            };
            return _repo.AddRoom(room);
            
        }

        public async Task UpdateRoom(RoomUpdateDTO request)
        {
            var room = await _repo.GetRoomById(request.Id);
            if (room == null)
                throw new KeyNotFoundException("Room not found");

            room.Name = request.Name ?? room.Name;
            room.VenueId = request.VenueId != 0 ? request.VenueId : room.VenueId;
            await _repo.UpdateRoom(room);
            return;
        }

        public Task DeleteRoom(long roomId) => _repo.DeleteRoom(roomId);
    }
}