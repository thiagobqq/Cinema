namespace Tickets.Domain.Enums
{
    internal enum PaymentStatus
    {
        Pending,
        Processing,
        Confirmed,
        Failed,
        Refunded,
        Expired
    }
}
