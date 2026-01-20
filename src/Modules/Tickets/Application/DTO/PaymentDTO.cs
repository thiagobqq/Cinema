using System;
using Tickets.Domain.Enums;

namespace Tickets.Application.DTO
{
    public class PaymentDTO
    {
        public long TicketId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }

    public class ProcessPaymentDTO
    {
        public long TicketId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public CardInfoDTO? CardInfo { get; set; }
        public PixInfoDTO? PixInfo { get; set; }
    }

    public class CardInfoDTO
    {
        public string CardNumber { get; set; } = string.Empty;
        public string CardHolder { get; set; } = string.Empty;
        public string ExpirationDate { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
    }

    public class PixInfoDTO
    {
        public string PixKey { get; set; } = string.Empty;
    }

    public class PaymentResponseDTO : PaymentDTO
    {
        public long Id { get; set; }
        public PaymentStatus Status { get; set; }
        public string? GatewayTransactionId { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public string? ErrorMessage { get; set; }
        public string? PixQrCode { get; set; }
        public string? PixCopyPaste { get; set; }
    }

    public class PaymentStatusUpdateDTO
    {
        public PaymentStatus Status { get; set; }
        public string? GatewayTransactionId { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class ProcessPaymentResponseDTO
    {
        public long PaymentId { get; set; }
        public string? GatewayTransactionId { get; set; }
        public PaymentStatus Status { get; set; }
        public string? Message { get; set; }
        public string? PixQrCode { get; set; }
        public string? PixCopyPaste { get; set; }
    }
}
