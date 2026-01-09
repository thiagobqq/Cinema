using System.Collections.Generic;
using System.Threading.Tasks;
using Movie.Domain.Models.impl;

namespace Movie.Domain.Interfaces.Repositories
{
    internal interface ISessionSeatRepository
    {
        Task<IEnumerable<SessionSeat>> GetAllSessionSeats();
        Task<SessionSeat?> GetSessionSeatById(long sessionSeatId);
        Task<IEnumerable<SessionSeat>> GetSessionSeatsBySessionId(long sessionId);
        Task AddSessionSeat(SessionSeat sessionSeat);
        Task UpdateSessionSeat(SessionSeat sessionSeat);
        Task DeleteSessionSeat(long sessionSeatId);
    }
}