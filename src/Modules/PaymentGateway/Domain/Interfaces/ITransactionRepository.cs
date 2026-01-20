using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(string id);
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<IEnumerable<Transaction>> GetByExternalReferenceAsync(string externalReference);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
    }
}
