namespace PaymentGateway.Application.DTOs;

public class PaymentRequestDto
{
    public string Payer { get; set; }
    public string Payee { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string? PayerReference { get; set; }
}