using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Application.DTO
{
    public class TransactionStatusDTO
    {
        public string TransactionId { get; set; } = string.Empty;
        public long ExternalReference { get; set; }
        public TransactionStatus Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
