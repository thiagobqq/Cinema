using System;
using Tickets.Domain.Enums;

namespace Tickets.Application.DTO
{
    public class TicketDTO
    {
        public string UserId { get; set; } = string.Empty;
        public long SessionSeatId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal Amount { get; set; }
    }

    public class TicketResponseDTO : TicketDTO
    {
        public long Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }

    public class TicketUpdateDTO : TicketResponseDTO { }

    public class BuyTicketDTO
    {
        public long SessionSeatId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
