using PaymentGateway.Application.DTO;

namespace PaymentGateway.Domain.Interfaces
{
    public interface IPaymentGatewayService
    {
        Task<bool> ProcessPaymentAsync(ProcessPaymentRequestDTO request);
        Task<TransactionStatusDTO?> GetTransactionStatusAsync(string transactionId);
        Task<RefundResponseDTO> RefundAsync(RefundRequestDTO request);
        Task<IEnumerable<TransactionStatusDTO>> GetAllTransactionsAsync();
    }
}
