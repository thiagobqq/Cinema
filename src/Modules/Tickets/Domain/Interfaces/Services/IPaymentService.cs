using System.Collections.Generic;
using System.Threading.Tasks;
using Tickets.Application.DTO;

namespace Tickets.Domain.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentResponseDTO>> GetAllPayments();
        Task<PaymentResponseDTO?> GetPaymentById(long paymentId);
        Task<IEnumerable<PaymentResponseDTO>> GetPaymentsByTicketId(long ticketId);
        Task CreatePayment(PaymentDTO payment);
        Task UpdatePaymentStatus(long paymentId, PaymentStatusUpdateDTO statusUpdate);
        Task<ProcessPaymentResponseDTO> ProcessPaymentAsync(ProcessPaymentDTO paymentDTO);
        Task<ProcessPaymentResponseDTO> RefundPaymentAsync(long paymentId, string? reason = null);
    }
}
