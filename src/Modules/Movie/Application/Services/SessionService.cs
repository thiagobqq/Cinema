using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movie.Application.DTO;
using Movie.Domain.Interfaces.Repositories;
using Movie.Domain.Interfaces.Services;
using Movie.Domain.Models.impl;

namespace Movie.Application.Services
{
    internal class SessionService : ISessionService
    {
        private readonly ISessionRepository _repo;

        public SessionService(ISessionRepository repo) => _repo = repo;

        public async Task<IEnumerable<SessionResponseDTO>> GetAllSessions()
        {
            var sessions = await _repo.GetAllSessions();
            return sessions.Select(s => new SessionResponseDTO
            {
                Id = s.Id,
                RoomId = s.RoomId,
                FilmId = s.FilmId,
                StartTime = s.StartsAt,
                EndTime = s.EndsAt,
                Price = s.Price
            });
        }

        public async Task<SessionResponseDTO?> GetSessionById(long sessionId)
        {
            var s = await _repo.GetSessionById(sessionId);
            if (s == null)
                throw new KeyNotFoundException("Session not found");

            return new SessionResponseDTO
            {
                Id = s.Id,
                RoomId = s.RoomId,
                FilmId = s.FilmId,
                StartTime = s.StartsAt,
                EndTime = s.EndsAt,
                Price = s.Price
            };
        }

        public Task AddSession(SessionDTO session)
        {
            var model = new Session
            {
                RoomId = session.RoomId,
                FilmId = session.FilmId,
                StartsAt = session.StartTime,
                EndsAt = session.EndTime,
                Price = session.Price
            };

            return _repo.AddSession(model);
        }

        public async Task UpdateSession(SessionUpdateDTO session)
        {
            var model = await _repo.GetSessionById(session.Id);
            if (model == null)
                throw new KeyNotFoundException("Session not found");

            model.RoomId = session.RoomId != 0 ? session.RoomId : model.RoomId;
            model.FilmId = session.FilmId != 0 ? session.FilmId : model.FilmId;
            model.StartsAt = session.StartTime != default ? session.StartTime : model.StartsAt;
            model.EndsAt = session.EndTime != default ? session.EndTime : model.EndsAt;
            model.Price = session.Price != 0 ? session.Price : model.Price;

            await _repo.UpdateSession(model);
        }

        public Task DeleteSession(long sessionId) => _repo.DeleteSession(sessionId);
    }
}