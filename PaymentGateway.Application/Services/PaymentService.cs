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
        var transaction = new Transaction
        (
            payer: request.Payer,
            payee: request.Payee,
            amount: request.Amount,
            currency: request.Currency
            //status: status,
        )
        {
            PaymentReference = request.PayerReference
        };

        await Task.Delay(150);

        var saved = _repository.AddTransactionAsync(transaction);
        return new PaymentResponseDto
        {
            TransactionReference = transaction.Id,
            StatusCode = status switch
            {
                "PENDING"=> 100,
                "SUCCESS"=> 200,
                "FAILURE"=> 400,
                _ => 400
            },
            Message = status switch
            {
                "PENDING" => "Transaction Pending",
                "SUCCESS" => "Transaction successfully processed",
                "FAILURE" => "Transaction Failed",
                _ => "Unknown Error"
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