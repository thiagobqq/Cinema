using System;
using Tickets.Domain.Enums;
using Tickets.Domain.Models;

namespace Tickets.Domain.Models.impl
{
    internal class Payment : Model
    {
        public long TicketId { get; set; }
        public Ticket Ticket { get; set; } = null!;
        
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }
        
        public string? GatewayTransactionId { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public string? ErrorMessage { get; set; }
        public string? PixQrCode { get; set; }
        public string? PixCopyPaste { get; set; }
    }
}
