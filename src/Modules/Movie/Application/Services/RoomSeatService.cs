using System.Collections.Generic;
using System.Threading.Tasks;
using Movie.Application.DTO;
using Movie.Domain.Interfaces.Repositories;
using Movie.Domain.Interfaces.Services;
using Movie.Domain.Models.impl;

namespace Movie.Application.Services
{
    internal class RoomSeatService : IRoomSeatService
    {
        private readonly IRoomSeatRepository _repo;

        public RoomSeatService(IRoomSeatRepository repo) => _repo = repo;

        public async Task<IEnumerable<RoomSeatDTO>> GetAllRoomSeats()
        {
            var roomSeats = await _repo.GetAllRoomSeats();

            return roomSeats.Select(rs => new RoomSeatDTO
            {
                RowLabel = rs.RowLabel,
                SeatNumber = rs.SeatNumber,
                IsActive = rs.IsActive,
                Type = rs.Type.ToString()
            });
        }

        public async Task<RoomSeatDTO?> GetRoomSeatById(RoomSeatRequestDTO request)
        {
            var roomSeat = await _repo.GetRoomSeatById(request.Id);
            if (roomSeat == null) 
                throw new KeyNotFoundException("Room seat not found.");

            return new RoomSeatDTO
            {
                RowLabel = roomSeat.RowLabel,
                SeatNumber = roomSeat.SeatNumber,
                IsActive = roomSeat.IsActive,
                Type = roomSeat.Type.ToString()
            };
        }

        public Task AddRoomSeat(RoomSeatDTO roomSeat)
        {
            var newRoomSeat = new RoomSeat
            {
                RowLabel = roomSeat.RowLabel,
                SeatNumber = roomSeat.SeatNumber,
                IsActive = roomSeat.IsActive,
                Type = Enum.Parse<Movie.Domain.Enums.SeatType>(roomSeat.Type)
            };
            return _repo.AddRoomSeat(newRoomSeat);
        }

        public async Task UpdateRoomSeat(RoomSeatUpdateDTO request)
        {
            var roomSeat = await _repo.GetRoomSeatById(request.Id);
            if (roomSeat == null)
                throw new KeyNotFoundException("Room seat not found.");

            roomSeat.RowLabel = request.RowLabel ?? roomSeat.RowLabel;
            roomSeat.SeatNumber = request.SeatNumber != 0 ? request.SeatNumber : roomSeat.SeatNumber;
            roomSeat.IsActive = request.IsActive;
            roomSeat.Type = !string.IsNullOrEmpty(request.Type) ? Enum.Parse<Movie.Domain.Enums.SeatType>(request.Type) : roomSeat.Type;

            await _repo.UpdateRoomSeat(roomSeat);
        }
        public Task DeleteRoomSeat(long roomSeatId) => _repo.DeleteRoomSeat(roomSeatId);
    }
}