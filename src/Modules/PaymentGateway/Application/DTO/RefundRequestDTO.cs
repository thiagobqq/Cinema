namespace PaymentGateway.Application.DTO
{
    public class RefundRequestDTO
    {
        public string TransactionId { get; set; } = string.Empty;
        public decimal? Amount { get; set; } // null = refund total
        public string? Reason { get; set; }
    }

    public class RefundResponseDTO
    {
        public string RefundId { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public decimal RefundedAmount { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
