using System.Collections.Generic;
using System.Threading.Tasks;
using Movie.Application.DTO;

namespace Movie.Domain.Interfaces.Services
{
    internal interface ISessionService
    {
        Task<IEnumerable<SessionResponseDTO>> GetAllSessions();
        Task<SessionResponseDTO?> GetSessionById(long sessionId);
        Task AddSession(SessionDTO session);
        Task UpdateSession(SessionUpdateDTO session);
        Task DeleteSession(long sessionId);
    }
}