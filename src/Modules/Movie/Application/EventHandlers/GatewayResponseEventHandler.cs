using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Movie.Application.DTO;
using Movie.Domain.Enums;
using Movie.Domain.Interfaces.Services;
using Movie.Domain.Models.impl;
using Shared.Events.impl;

namespace Movie.Application.EventHandlers
{
    public class GatewayResponseEventHandler : INotificationHandler<GatewayResponseEvent>
    {
        private readonly ISessionSeatService _seatService;

        public GatewayResponseEventHandler(ISessionSeatService seatService)
        {
            _seatService = seatService;
        }
        public async Task Handle(GatewayResponseEvent notification, CancellationToken cancellationToken)
        {
            var seat = await _seatService.GetSessionSeatById(notification.SessionSeatId);
            if (seat == null)
                return;

            var updatedSeat = new SessionSeatUpdateDTO
            {
                Id = notification.SessionSeatId,
                SessionId = seat.SessionId,
                RoomSeatId = seat.RoomSeatId,
                Status = MapPaymentStatusToSeatStatus(notification.Status),
                TicketCode = notification.TicketCode,
                ReservedUntil = null
            };

            await _seatService.UpdateSessionSeat(updatedSeat);                       
        }

        private string MapPaymentStatusToSeatStatus(string paymentStatus)
        {
            return paymentStatus switch
            {
                "Confirmed" => SeatStatus.Sold.ToString(),
                "Processing" => SeatStatus.Locked.ToString(),
                "Failed" => SeatStatus.Available.ToString(),
                "Refunded" => SeatStatus.Available.ToString(),
                _ => SeatStatus.Available.ToString()
            };
        }
    }
}