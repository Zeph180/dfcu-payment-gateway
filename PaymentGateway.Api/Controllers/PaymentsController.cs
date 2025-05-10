using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.DTOs;
using PaymentGateway.Application.Services;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Api.Controllers;

public class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentsController"/> class.
    /// </summary>
    /// <param name="paymentService">Service for processing payments.</param>
    public PaymentsController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }
    
    /// <summary>
    /// Initiates a new payment process.
    /// </summary>
    /// <param name="request">The payment request payload.</param>
    /// <returns>An HTTP response with the payment processing result.</returns>
    /// <response code="200">Payment processed successfully.</response>
    /// <response code="400">Invalid request or processing failure.</response>
    [HttpPost("initiate")]
    public async Task<IActionResult> Initiate([FromBody] PaymentRequestDto request)
    {
        var response = await _paymentService.ProcessPaymentAsync(request);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Gets the status of a transaction by its ID.
    /// </summary>
    /// <param name="repo">Transaction repository instance (injected).</param>
    /// <param name="transactionId">The unique identifier of the transaction.</param>
    /// <returns>An HTTP response with transaction details or a not found error.</returns>
    /// <response code="200">Transaction found and returned successfully.</response>
    /// <response code="404">Transaction not found.</response>
    [HttpGet("status/{transactionId}")]
    public async Task<IActionResult> GetStatus([FromServices] ITransactionRepository repo, Guid transactionId)
    {
        var transaction = await repo.GetByReferenceAsync(transactionId);
        if (transaction == null)
            return NotFound("Transaction not found");

        return Ok(new
        {
            Reference = transaction.Id,
            Status = transaction.Status,
            CreatedAt = transaction.CreatedAt
        });
    }
} 