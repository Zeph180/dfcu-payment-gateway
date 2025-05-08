using PaymentGateway.Domain;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Infrastructure.Data;

namespace PaymentGateway.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Transaction> AddTransactionAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<Transaction?> GetByReferenceAsync(Guid transactionId)
    {
        return await _context.Transactions.FindAsync(transactionId);
    }
}