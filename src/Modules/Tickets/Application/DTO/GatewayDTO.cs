using Tickets.Domain.Enums;

namespace Tickets.Application.DTO
{
    // DTOs para comunicação com o PaymentGateway via HTTP
    public class GatewayProcessPaymentRequest
    {
        public string ExternalReference { get; set; } = string.Empty;
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
        public string ExternalReference { get; set; } = string.Empty;
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

    public class GatewayRefundRequest
    {
        public string TransactionId { get; set; } = string.Empty;
        public decimal? Amount { get; set; }
        public string? Reason { get; set; }
    }

    public class GatewayRefundResponse
    {
        public string RefundId { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public decimal RefundedAmount { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
