using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movie.Application.DTO;
using Movie.Domain.Enums;
using Movie.Domain.Interfaces.Repositories;
using Movie.Domain.Interfaces.Services;
using Movie.Domain.Models.impl;

namespace Movie.Application.Services
{
    internal class SessionSeatService : ISessionSeatService
    {
        private readonly ISessionSeatRepository _repo;

        public SessionSeatService(ISessionSeatRepository repo) => _repo = repo;

        public async Task<IEnumerable<SessionSeatDTO>> GetAllSessionSeats()
        {
            var seats = await _repo.GetAllSessionSeats();
            return seats.Select(s => new SessionSeatDTO
            {
                SessionId = s.SessionId,
                RoomSeatId = s.RoomSeatId,
                Status = s.Status.ToString(),
                ReservedUntil = s.ReservedUntil,
                TicketCode = s.TicketCode
            });
        }

        public async Task<SessionSeatDTO?> GetSessionSeatById(SessionSeatRequestDTO request)
        {
            var s = await _repo.GetSessionSeatById(request.Id);
            if (s == null)
                throw new KeyNotFoundException("Session seat not found.");

            return new SessionSeatDTO
            {
                SessionId = s.SessionId,
                RoomSeatId = s.RoomSeatId,
                Status = s.Status.ToString(),
                ReservedUntil = s.ReservedUntil,
                TicketCode = s.TicketCode
            };
        }

        public async Task<IEnumerable<SessionSeatDTO>> GetSessionSeatsBySessionId(long sessionId)
        {
            var seats = await _repo.GetSessionSeatsBySessionId(sessionId);
            return seats.Select(s => new SessionSeatDTO
            {
                SessionId = s.SessionId,
                RoomSeatId = s.RoomSeatId,
                Status = s.Status.ToString(),
                ReservedUntil = s.ReservedUntil,
                TicketCode = s.TicketCode
            });
        }

        public Task AddSessionSeat(SessionSeatDTO sessionSeat)
        {
            var model = new SessionSeat
            {
                SessionId = sessionSeat.SessionId,
                RoomSeatId = sessionSeat.RoomSeatId,
                Status = Enum.Parse<Movie.Domain.Enums.SeatStatus>(sessionSeat.Status),
                ReservedUntil = sessionSeat.ReservedUntil,
                TicketCode = sessionSeat.TicketCode
            };
            return _repo.AddSessionSeat(model);
        }

        public async Task UpdateSessionSeat(SessionSeatUpdateDTO request)
        {
            var model = await _repo.GetSessionSeatById(request.Id);
            if (model == null)
                throw new KeyNotFoundException("Session seat not found.");

            model.SessionId = request.SessionId != 0 ? request.SessionId : model.SessionId;
            model.RoomSeatId = request.RoomSeatId != 0 ? request.RoomSeatId : model.RoomSeatId;
            model.Status = !string.IsNullOrEmpty(request.Status) ? Enum.Parse<Movie.Domain.Enums.SeatStatus>(request.Status) : model.Status;
            model.ReservedUntil = request.ReservedUntil ?? model.ReservedUntil;
            model.TicketCode = request.TicketCode ?? model.TicketCode;

            await _repo.UpdateSessionSeat(model);
        }

        public Task DeleteSessionSeat(long sessionSeatId) => _repo.DeleteSessionSeat(sessionSeatId);

        public async Task<bool> IsSeatAvailable(long sessionSeatId)
        {
            var seat = await _repo.GetSessionSeatById(sessionSeatId);
            if (seat == null)
                return false;

            return seat.Status == SeatStatus.Available
                   && (!seat.ReservedUntil.HasValue || seat.ReservedUntil <= DateTime.UtcNow);
        }
    }
}