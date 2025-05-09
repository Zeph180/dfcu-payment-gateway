using PaymentGateway.Application.DTOs;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Application.Services;

public class PaymentService
{
    private readonly ITransactionRepository _repository;

    public PaymentService(ITransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto request)
    {
        var status = GetRandomStatus();

        var result = Transaction.Create(
            payer: request.Payer,
            payee: request.Payee,
            amount: request.Amount,
            currency: request.Currency,
            payerReference: request.PayerReference
        );

        if (!result.Success)
        {
            return new PaymentResponseDto
            {
                TransactionReference = Guid.Empty,
                StatusCode = 400,
                Message = result.Message
            };
        }

        var transaction = result.Data;

        await Task.Delay(100);

        await _repository.AddTransactionAsync(transaction);

        return new PaymentResponseDto
        {
            TransactionReference = transaction.Id,
            StatusCode = status switch
            {
                "PENDING" => 100,
                "SUCCESS" => 200,
                "FAILED" => 400,
                _ => 400
            },
            Message = status switch
            {
                "PENDING" => "Transaction Pending",
                "SUCCESS" => "Transaction successfully processed",
                "FAILED" => "Transaction failed",
                _ => "Unknown error"
            }
        };
    }

    private string GetRandomStatus()
    {
        var random = new Random().Next(1, 101);
        if (random <= 10) return "PENDING";
        if (random <= 95) return "SUCCESS";
        return "FAILURE";
    }
}