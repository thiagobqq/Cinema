using System.Collections.Generic;
using System.Threading.Tasks;
using Tickets.Domain.Models.impl;

namespace Tickets.Domain.Interfaces.Repositories
{
    internal interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllPayments();
        Task<Payment?> GetPaymentById(long paymentId);
        Task<IEnumerable<Payment>> GetPaymentsByTicketId(long ticketId);
        Task AddPayment(Payment payment);
        Task UpdatePayment(Payment payment);
    }
}
