using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Infra.Data;

namespace PaymentGateway.Infra.Repository
{
    internal class TransactionRepository : ITransactionRepository
    {
        private readonly PaymentGatewayDbContext _context;

        public TransactionRepository(PaymentGatewayDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction?> GetByIdAsync(string id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _context.Transactions
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByExternalReferenceAsync(string externalReference)
        {
            return await _context.Transactions
                .Where(t => t.ExternalReference == externalReference)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
