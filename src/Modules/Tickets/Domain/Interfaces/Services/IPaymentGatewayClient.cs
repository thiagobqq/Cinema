using Tickets.Application.DTO;

namespace Tickets.Domain.Interfaces.Services
{
    public interface IPaymentGatewayClient
    {
        Task<GatewayProcessPaymentResponse> ProcessPaymentAsync(GatewayProcessPaymentRequest request);
        Task<GatewayTransactionStatus?> GetTransactionStatusAsync(string transactionId);
        Task<GatewayRefundResponse> RefundAsync(GatewayRefundRequest request);
    }
}
