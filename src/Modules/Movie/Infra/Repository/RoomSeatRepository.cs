using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movie.Domain.Interfaces.Repositories;
using Movie.Domain.Models.impl;
using Movie.Infra.Data;

namespace Movie.Infra.Repository
{
    internal class RoomSeatRepository : IRoomSeatRepository
    {
        private readonly MovieDbContext _context;

        public RoomSeatRepository(MovieDbContext context) => _context = context;

        public async Task<IEnumerable<RoomSeat>> GetAllRoomSeats()
        {
            return await _context.RoomSeats
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<RoomSeat?> GetRoomSeatById(long roomSeatId)
        {
            return await _context.RoomSeats
                .AsNoTracking()
                .FirstOrDefaultAsync(rs => rs.Id == roomSeatId);
        }

        public async Task AddRoomSeat(RoomSeat roomSeat)
        {
            await _context.RoomSeats.AddAsync(roomSeat);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoomSeat(RoomSeat roomSeat)
        {
            _context.RoomSeats.Update(roomSeat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoomSeat(long roomSeatId)
        {
            var seat = await _context.RoomSeats.FindAsync(roomSeatId);
            if (seat != null)
            {
                _context.RoomSeats.Remove(seat);
                await _context.SaveChangesAsync();
            }
        }
    }
}