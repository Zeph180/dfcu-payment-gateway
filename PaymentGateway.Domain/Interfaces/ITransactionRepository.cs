namespace PaymentGateway.Domain.Interfaces;

public interface ITransactionRepository
{
    /// <summary>
    /// Asynchronously adds a new transaction to the data store.
    /// </summary>
    /// <param name="transaction">The transaction to be added.</param>
    /// <returns>The added transaction including any updated fields (e.g., ID).</returns>
    Task<Transaction> AddTransactionAsync(Transaction transaction);
    
    /// <summary>
    /// Asynchronously retrieves a transaction by its unique identifier.
    /// </summary>
    /// <param name="transactionId">The unique identifier of the transaction.</param>
    /// <returns>The transaction if found; otherwise, null.</returns>
    Task<Transaction> GetByReferenceAsync(Guid transactionId);
}