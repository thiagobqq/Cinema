using System;
using Tickets.Domain.Enums;

namespace Tickets.Application.DTO
{
    internal class PaymentDTO
    {
        public long TicketId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }

    internal class PaymentResponseDTO : PaymentDTO
    {
        public long Id { get; set; }
        public PaymentStatus Status { get; set; }
        public string? GatewayTransactionId { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public string? ErrorMessage { get; set; }
    }

    internal class PaymentStatusUpdateDTO
    {
        public PaymentStatus Status { get; set; }
        public string? GatewayTransactionId { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
