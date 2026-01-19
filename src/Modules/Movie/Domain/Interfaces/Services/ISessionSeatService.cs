using System.Collections.Generic;
using System.Threading.Tasks;
using Movie.Application.DTO;

namespace Movie.Domain.Interfaces.Services
{
    public interface ISessionSeatService
    {
        Task<IEnumerable<SessionSeatDTO>> GetAllSessionSeats();
        Task<SessionSeatDTO?> GetSessionSeatById(SessionSeatRequestDTO request);
        Task<IEnumerable<SessionSeatDTO>> GetSessionSeatsBySessionId(long sessionId);
        Task AddSessionSeat(SessionSeatDTO sessionSeat);
        Task UpdateSessionSeat(SessionSeatUpdateDTO sessionSeat);
        Task DeleteSessionSeat(long sessionSeatId);
    }
}