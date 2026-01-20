using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Application.DTO
{
    public class ProcessPaymentResponseDTO
    {
        public string TransactionId { get; set; } = string.Empty;
        public string ExternalReference { get; set; } = string.Empty;
        public TransactionStatus Status { get; set; }
        public decimal Amount { get; set; }
        public string? Message { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string? PixQrCode { get; set; }
        public string? PixCopyPaste { get; set; }
    }
}
