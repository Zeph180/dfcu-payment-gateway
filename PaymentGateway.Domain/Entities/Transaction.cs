
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.Common;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Domain;

/// <summary>
/// Represents a financial transaction between a payer and a payee.
/// </summary>
public class Transaction
{
    private static ILogger<Transaction>? _logger;

    /// <summary>
    /// Sets the logger instance for the Transaction class.
    /// </summary>
    /// <param name="logger">Logger instance to use</param>
    public static void ConfigureLogger(ILogger<Transaction> logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// Unique identifier for the transaction.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// 10-digit numeric account number of the payer.
    /// </summary>
    public string Payer { get; set; } = string.Empty;
    
    /// <summary>
    /// 10-digit numeric account number of the payee.
    /// </summary>
    public string Payee { get; set; } = string.Empty;
    
    /// <summary>
    /// Amount to be transferred in the transaction.
    /// </summary>
    public decimal Amount { get; set; } = decimal.Zero;
    
    /// <summary>
    /// 3-letter ISO currency code (e.g., USD, EUR).
    /// </summary>
    public string Currency { get; set; } = string.Empty;
    
    /// <summary>
    /// Optional reference provided by the payer.
    /// </summary>
    public string? PaymentReference { get; set; } = null;
    
    /// <summary>
    /// Current status of the transaction (e.g., Pending, Completed).
    /// </summary>
    /// 
    public string Status { get; set; }
    
    /// <summary>
    /// Timestamp of when the transaction was created (in UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Private constructor to enforce the use of the Create method.
    /// </summary>
    private Transaction() { }

    
    /// <summary>
    /// Factory method for creating a new Transaction instance with validation.
    /// </summary>
    /// <param name="payer">Payer's 10-digit numeric account number.</param>
    /// <param name="payee">Payee's 10-digit numeric account number.</param>
    /// <param name="amount">Transaction amount (must be greater than zero).</param>
    /// <param name="currency">3-letter ISO currency code.</param>
    /// <param name="status">Transaction status.</param>
    /// <param name="payerReference">Optional reference from the payer.</param>
    /// <returns>Result containing the transaction or a validation error message.</returns>
    public static Result<Transaction> Create(
        string payer,
        string payee,
        decimal amount,
        string currency,
        string status,
        string? payerReference = null)
    {
        try
        {
            _logger?.LogDebug("Attempting to create new transaction with parameters: " +
                              "Payer: {Payer}, Payee: {Payee}, Amount: {Amount}, " +
                              "Currency: {Currency}, Status: {Status}, Reference: {Reference}",
                payer, payee, amount, currency, status, payerReference);
        
            if (string.IsNullOrWhiteSpace(payer) || !payer.All(char.IsDigit) || payer.Length != 10)
                return Result<Transaction>.Fail("Payer must be a numeric 10-digit account number.");

            if (string.IsNullOrWhiteSpace(payee) || !payee.All(char.IsDigit) || payee.Length != 10)
                return Result<Transaction>.Fail("Payee must be a numeric 10-digit account number.");

            if (amount <= 0)
                return Result<Transaction>.Fail("Amount must be greater than zero.");

            if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
                return Result<Transaction>.Fail("Currency must be a 3-letter ISO code.");

            var transaction = new Transaction
            {
                Payer = payer,
                Payee = payee,
                Amount = amount,
                Currency = currency,
                PaymentReference = payerReference,
                Status = status,
            };

            _logger?.LogInformation("Successfully created transaction {TransactionId}", transaction.Id);
            _logger?.LogDebug("Created transaction details: {@Transaction}", transaction);
            return Result<Transaction>.Ok(transaction);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Unexpected error occurred while creating transaction. " +
                                  "Parameters: Payer: {Payer}, Payee: {Payee}, Amount: {Amount}, " +
                                  "Currency: {Currency}, Status: {Status}, Reference: {Reference}",
                payer, payee, amount, currency, status, payerReference);
            return Result<Transaction>.Fail("An unexpected error occurred while creating the transaction.");
        }
    }
}
