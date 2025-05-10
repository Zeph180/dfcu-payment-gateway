
using PaymentGateway.Domain.Common;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Domain;
public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Payer { get; set; } = string.Empty;
    public string Payee { get; set; } = string.Empty;
    public decimal Amount { get; set; } = decimal.Zero;
    public string Currency { get; set; } = string.Empty;
    public string? PaymentReference { get; set; } = null;
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    private Transaction() { }

    public static Result<Transaction> Create(
        string payer,
        string payee,
        decimal amount,
        string currency,
        string status,
        string? payerReference = null)
    {
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

        return Result<Transaction>.Ok(transaction);
    }
}
