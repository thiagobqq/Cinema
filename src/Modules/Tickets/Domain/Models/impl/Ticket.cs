using System;
using Tickets.Domain.Enums;
using Tickets.Domain.Models;

namespace Tickets.Domain.Models.impl
{
    internal class Ticket : Model
    {
        public string UserId { get; set; } = string.Empty;
        public long SessionSeatId { get; set; }
        
        public string Code { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal Amount { get; set; }
        
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
