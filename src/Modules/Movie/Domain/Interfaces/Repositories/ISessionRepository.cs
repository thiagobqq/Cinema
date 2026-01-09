using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movie.Domain.Models.impl;

namespace Movie.Domain.Interfaces.Repositories
{
    internal interface ISessionRepository
    {
        Task<IEnumerable<Session>> GetAllSessions();
        Task<Session?> GetSessionById(long sessionId);
        Task AddSession(Session session);
        Task UpdateSession(Session session);
        Task DeleteSession(long sessionId);
    }
}