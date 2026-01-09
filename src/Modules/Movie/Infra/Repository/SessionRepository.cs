using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movie.Domain.Interfaces.Repositories;
using Movie.Domain.Models.impl;
using Movie.Infra.Data;

namespace Movie.Infra.Repository
{
    internal class SessionRepository : ISessionRepository
    {
        private readonly MovieDbContext _context;

        public SessionRepository(MovieDbContext context) => _context = context;

        public async Task<IEnumerable<Session>> GetAllSessions()
        {
            return await _context.Sessions
                .AsNoTracking()
                .Include(s => s.Film)
                .Include(s => s.Room)
                .Include(s => s.OccupiedSeats)
                    .ThenInclude(ss => ss.RoomSeat)
                .ToListAsync();
        }

        public async Task<Session?> GetSessionById(long sessionId)
        {
            return await _context.Sessions
                .Include(s => s.Film)
                .Include(s => s.Room)
                    .ThenInclude(r => r.Seats)
                .Include(s => s.OccupiedSeats)
                    .ThenInclude(ss => ss.RoomSeat)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == sessionId);
        }

        public async Task AddSession(Session session)
        {
            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSession(Session session)
        {
            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSession(long sessionId)
        {
            var session = await _context.Sessions.FindAsync(sessionId);
            if (session != null)
            {
                _context.Sessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }
    }
}