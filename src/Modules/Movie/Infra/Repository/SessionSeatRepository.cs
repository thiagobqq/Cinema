using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movie.Application.DTO;
using Movie.Domain.Enums;
using Movie.Domain.Interfaces.Repositories;
using Movie.Domain.Models.impl;
using Movie.Infra.Data;

namespace Movie.Infra.Repository
{
    internal class SessionSeatRepository : ISessionSeatRepository
    {
        private readonly MovieDbContext _context;

        public SessionSeatRepository(MovieDbContext context) => _context = context;

        public async Task<IEnumerable<SessionSeat>> GetAllSessionSeats()
        {
            return await _context.SessionSeats
                .AsNoTracking()
                .Include(ss => ss.Session)
                    .ThenInclude(s => s.Film)
                .Include(ss => ss.RoomSeat)
                .ToListAsync();
        }

        public async Task<SessionSeat?> GetSessionSeatById(long sessionSeatId)
        {
            return await _context.SessionSeats
                .Include(ss => ss.Session)
                    .ThenInclude(s => s.Room)
                .Include(ss => ss.RoomSeat)
                .AsNoTracking()
                .FirstOrDefaultAsync(ss => ss.Id == sessionSeatId);
        }

        public async Task<IEnumerable<SessionSeat>> GetSessionSeatsBySessionId(long sessionId)
        {
            return await _context.SessionSeats
                .AsNoTracking()
                .Where(ss => ss.SessionId == sessionId)
                .Include(ss => ss.RoomSeat)
                .ToListAsync();
        }

        public async Task AddSessionSeat(SessionSeat sessionSeat)
        {
            await _context.SessionSeats.AddAsync(sessionSeat);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSessionSeat(SessionSeat sessionSeat)
        {
            await _context.SessionSeats
                .Where(ss => ss.Id == sessionSeat.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(ss => ss.Status, sessionSeat.Status)
                    .SetProperty(ss => ss.ReservedUntil, sessionSeat.ReservedUntil)
                    .SetProperty(ss => ss.TicketCode, sessionSeat.TicketCode)
                    .SetProperty(ss => ss.UpdatedAt, DateTime.UtcNow)
                );
        }
        

        public async Task DeleteSessionSeat(long sessionSeatId)
        {
            var ss = await _context.SessionSeats.FindAsync(sessionSeatId);
            if (ss != null)
            {
                _context.SessionSeats.Remove(ss);
                await _context.SaveChangesAsync();
            }
        }

        private SeatStatus mapStatusFromGateway(string status)
        {
            return status switch
            {
                "Confirmed" => SeatStatus.Sold,
                "Failed" => SeatStatus.Available,
                "Refunded" => SeatStatus.Available,
                "Processing" => SeatStatus.Locked,
                _ => SeatStatus.Available
            };
        }
    }
}