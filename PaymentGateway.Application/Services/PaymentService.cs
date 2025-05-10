using PaymentGateway.Application.DTOs;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Application.Services;

/// <summary>
/// Handles the business logic for processing payments.
/// </summary>
public class PaymentService
{
    private readonly ITransactionRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentService"/> class.
    /// </summary>
    /// <param name="repository">The transaction repository used for persistence.</param>
    public PaymentService(ITransactionRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Processes a payment request and returns the transaction response.
    /// </summary>
    /// <param name="request">The payment request DTO containing payer/payee details and amount.</param>
    /// <returns>A <see cref="PaymentResponseDto"/> indicating the result of the operation.</returns>
    public async Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto request)
    {
        var status = GetRandomStatus();

        var result = Transaction.Create(
            payer: request.Payer,
            payee: request.Payee,
            amount: request.Amount,
            currency: request.Currency,
            status: status,
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

    /// <summary>
    /// Randomly determines a transaction status to simulate varying outcomes.
    /// </summary>
    /// <returns>A string status: PENDING, SUCCESS, or FAILURE.</returns>
    private string GetRandomStatus()
    {
        var random = new Random().Next(1, 101);
        if (random <= 10) return "PENDING";
        if (random <= 95) return "SUCCESS";
        return "FAILURE";
    }
}