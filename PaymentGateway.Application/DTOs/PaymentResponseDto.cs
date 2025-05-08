namespace PaymentGateway.Application.DTOs;

public class PaymentResponseDto
{
    public Guid TransactionReference { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
}