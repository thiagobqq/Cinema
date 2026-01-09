using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movie.Domain.Interfaces.Repositories;
using Movie.Domain.Models.impl;
using Movie.Infra.Data;

namespace Movie.Infra.Repository
{
    internal class RoomRepository : IRoomRepository
    {
        private readonly MovieDbContext _context;

        public RoomRepository(MovieDbContext context) => _context = context;

        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            return await _context.Rooms
                .AsNoTracking()
                .Include(r => r.Seats)
                .Include(r => r.Sessions)
                .ToListAsync();
        }

        public async Task<Room?> GetRoomById(long roomId)
        {
            return await _context.Rooms
                .Include(r => r.Venue)
                .Include(r => r.Seats)
                .Include(r => r.Sessions)
                    .ThenInclude(s => s.OccupiedSeats)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == roomId);
        }

        public async Task AddRoom(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoom(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoom(long roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
        }
    }
}