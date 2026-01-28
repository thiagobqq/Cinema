using Tickets.Domain.Enums;

namespace Tickets.Application.DTO
{
    public class GatewayProcessPaymentRequestDTO
    {
        public long ExternalReference { get; set; }
        public decimal Amount { get; set; }
        public int PaymentMethod { get; set; }
        public GatewayCardInfo? CardInfo { get; set; }
        public GatewayPixInfo? PixInfo { get; set; }
    }

    public class GatewayCardInfo
    {
        public string CardNumber { get; set; } = string.Empty;
        public string CardHolder { get; set; } = string.Empty;
        public string ExpirationDate { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
    }

    public class GatewayPixInfo
    {
        public string PixKey { get; set; } = string.Empty;
    }

    public class GatewayProcessPaymentResponse
    {
        public string TransactionId { get; set; } = string.Empty;
        public long ExternalReference { get; set; }
        public int Status { get; set; }
        public decimal Amount { get; set; }
        public string? Message { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string? PixQrCode { get; set; }
        public string? PixCopyPaste { get; set; }
    }

    public class GatewayTransactionStatus
    {
        public string TransactionId { get; set; } = string.Empty;
        public string ExternalReference { get; set; } = string.Empty;
        public int Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class GatewayRefundRequestDTO
    {
        public string TransactionId { get; set; } = string.Empty;
        public decimal? Amount { get; set; }
        public string? Reason { get; set; }
    }

    public class GatewayRefundResponseDTO
    {
        public string RefundId { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public decimal RefundedAmount { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public DateTime ProcessedAt { get; set; }
    }

    public class GatewayWebhookDTO
    {
        public string TransactionId { get; set; } = string.Empty;
        public long ExternalReference { get; set; }
        public GatewayCustomerDTO? Customer { get; set; }
        public int Status { get; set; }
        public decimal Amount { get; set; }
        public string? Message { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string? PixQrCode { get; set; }
        public string? PixCopyPaste { get; set; }
    }

    public class GatewayCustomerDTO
    {
        public string CustomerId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public enum GatewayStatus
    {
        Pending = 0,
        Processing = 1,
        Approved = 2,
        Declined = 3,
        Error = 4,
        Refunded = 5
    }
}
