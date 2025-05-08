
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Domain;

public class Transaction
{
    public Guid Id { get; set; } = new Guid();
    public string Payer { get; set; } = string.Empty;
    public string Payee { get; set; } = string.Empty;
    public decimal Amount { get; set; } = decimal.Zero;
    public string Currency { get; set; } = string.Empty;
    public string? PaymentReference { get; set; } = null;
    public TransactionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    private Transaction() {}
    
    public Transaction(
        string payer, 
        string payee, 
        decimal amount, 
        string currency, 
        string? payerReference = null)
    {
        Payer = payer ?? throw new ArgumentNullException(nameof(payer));
        Payee = payee ?? throw new ArgumentNullException(nameof(payee));
        Amount = amount > 0 ? amount : throw new ArgumentException("Amount must be positive");
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        PaymentReference = payerReference;
        Status = TransactionStatus.Pending;
    }

    public void MarkAsCompleted() => Status = TransactionStatus.Completed;
    public void MarkAsFailed() => Status = TransactionStatus.Failed;
}