using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Infrastructure.Data;

namespace PaymentGateway.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for managing Transaction entities in the database.
/// Uses stored procedures for all database operations.
/// </summary>
public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<TransactionRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionRepository"/> class.
    /// </summary>
    /// <param name="context">The database context for accessing the database.</param>
    /// <param name="logger">Logger instance for logging operations.</param>
    public TransactionRepository(AppDbContext context, ILogger<TransactionRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    /// <summary>
    /// Adds a new transaction to the database.
    /// </summary>
    /// <param name="transaction">The transaction to add.</param>
    /// <returns>The added transaction.</returns>
    /// <exception cref="Exception">Thrown when database operation fails.</exception>
    public async Task<Transaction> AddTransactionAsync(Transaction transaction)
    {
        var sql = "EXEC sp_AddTransaction @Id, @Payer, @Payee, @Amount, @Currency, @PaymentReference, @Status, @CreatedAt";
        var parameters = new[]
        {
            new SqlParameter("@Id", transaction.Id),
            new SqlParameter("@Payer", transaction.Payer),
            new SqlParameter("@Payee", transaction.Payee),
            new SqlParameter("@Amount", transaction.Amount),
            new SqlParameter("@Currency", transaction.Currency),
            new SqlParameter("@PaymentReference", transaction.PaymentReference),
            new SqlParameter("@Status", transaction.Status),
            new SqlParameter("@CreatedAt", transaction.CreatedAt)
        };

        try
        {
            _logger.LogInformation("Adding transaction: {@Transaction}", transaction);
            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            _logger.LogInformation("Transaction added successfully: {TransactionId}", transaction.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add transaction: {@Transaction}", transaction);
            throw;
        }

        return transaction;
    }

    /// <summary>
    /// Retrieves a transaction by its unique reference ID.
    /// </summary>
    /// <param name="transactionId">The unique identifier of the transaction.</param>
    /// <returns>The transaction if found; otherwise, null.</returns>
    /// <exception cref="Exception">Thrown when database operation fails.</exception>
    public async Task<Transaction?> GetByReferenceAsync(Guid transactionId)
    {
        var connection = _context.Database.GetDbConnection();

        try
        {
            _logger.LogInformation("Retrieving transaction by ID: {TransactionId}", transactionId);

            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = "sp_GetTransactionById";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@Id", transactionId));

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var result = Transaction.Create(
                    payer: reader.GetString(reader.GetOrdinal("Payer")),
                    payee: reader.GetString(reader.GetOrdinal("Payee")),
                    amount: reader.GetDecimal(reader.GetOrdinal("Amount")),
                    currency: reader.GetString(reader.GetOrdinal("Currency")),
                    status: reader.GetString(reader.GetOrdinal("Status")),
                    payerReference: reader.GetString(reader.GetOrdinal("PaymentReference"))
                );

                if (result.Success)
                {
                    _logger.LogInformation("Transaction retrieved successfully: {TransactionId}", transactionId);
                    return result.Data;
                }

                _logger.LogWarning("Failed to parse transaction: {TransactionId}", transactionId);
            }
            else
            {
                _logger.LogWarning("No transaction found for ID: {TransactionId}", transactionId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving transaction with ID: {TransactionId}", transactionId);
            throw;
        }
        finally
        {
            if (connection.State == System.Data.ConnectionState.Open)
                await connection.CloseAsync();
        }

        return null;
    }
}
