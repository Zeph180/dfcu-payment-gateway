namespace PaymentGateway.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction> AddTransactionAsync(Transaction transaction);
    Task<Transaction> GetByReferenceAsync(Guid transactionId);
}