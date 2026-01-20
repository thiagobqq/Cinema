using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Domain.Models.impl;
using Tickets.Infra.Data;

namespace Tickets.Infra.Repository
{
    internal class PaymentRepository : IPaymentRepository
    {
        private readonly TicketsDbContext _context;

        public PaymentRepository(TicketsDbContext context) => _context = context;

        public async Task<IEnumerable<Payment>> GetAllPayments() => await _context.Payments.ToListAsync();

        public async Task<Payment?> GetPaymentById(long paymentId) => await _context.Payments.FindAsync(paymentId);

        public async Task<IEnumerable<Payment>> GetPaymentsByTicketId(long ticketId) =>
            await _context.Payments.Where(p => p.TicketId == ticketId).ToListAsync();

        public async Task AddPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePayment(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

    }
}
