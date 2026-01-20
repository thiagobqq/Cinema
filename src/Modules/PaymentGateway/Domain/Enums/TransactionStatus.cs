namespace PaymentGateway.Domain.Enums
{
    public enum TransactionStatus
    {
        Pending,
        Processing,
        Approved,
        Declined,
        Error,
        Refunded
    }
}
