namespace PaymentGateway.Domain;

public class Transaction
{
    public Guid Id { get; set; } = new Guid();
    public string Payer { get; set; } = string.Empty;
    public string Payee { get; set; } = string.Empty;
    public decimal Amount { get; set; } = decimal.Zero;
    public string Currency { get; set; } = string.Empty;
    public string? PaymentReference { get; set; } = null;
    public string status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}