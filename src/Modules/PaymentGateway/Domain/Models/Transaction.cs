using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Domain.Models
{
    public class Transaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string ExternalReference { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public GatewayPaymentMethod PaymentMethod { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }
        public string? ErrorMessage { get; set; }
        public string? PixQrCode { get; set; }
        public string? PixCopyPaste { get; set; }
        public decimal RefundedAmount { get; set; }
    }
}
