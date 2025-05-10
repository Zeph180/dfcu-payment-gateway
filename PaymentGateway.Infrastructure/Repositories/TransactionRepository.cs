using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        return transaction;
    }
    
    public async Task<Transaction?> GetByReferenceAsync(Guid transactionId)
    {
        var connection = _context.Database.GetDbConnection();
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

            return result.Success ? result.Data : null;
        }

        return null;
    }

}