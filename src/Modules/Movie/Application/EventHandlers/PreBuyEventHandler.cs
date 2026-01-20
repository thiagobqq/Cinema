using MediatR;
using Cinema.Events.impl;
using Movie.Domain.Interfaces.Repositories;
using Movie.Domain.Enums;
using Movie.Domain.Interfaces.Services;
using Movie.Application.DTO;

namespace Movie.Application.EventHandlers
{
    public class PreBuyEventHandler : INotificationHandler<PreBuyEvent>
    {
        private readonly ISessionSeatService _seatService;

        public PreBuyEventHandler(ISessionSeatService seatService)
        {
            _seatService = seatService;
        }

        public async Task Handle(PreBuyEvent notification, CancellationToken cancellationToken)
        {
            var seat = await _seatService.GetSessionSeatById(notification.SessionSeatId);
            if (seat == null)
                return;

            var updatedSeat = new SessionSeatUpdateDTO
            {
                Id = notification.SessionSeatId,
                SessionId = seat.SessionId,
                RoomSeatId = seat.RoomSeatId,
                Status = SeatStatus.Locked.ToString(),
                ReservedUntil = DateTime.UtcNow.AddMinutes(15)
            };

            await _seatService.UpdateSessionSeat(updatedSeat);
        }
    }
}
