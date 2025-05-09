using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.DTOs;
using PaymentGateway.Application.Services;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Api.Controllers;

public class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;
    
    public PaymentsController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }
    
    [HttpPost("initiate")]
    public async Task<IActionResult> Initiate([FromBody] PaymentRequestDto request)
    {
        var response = await _paymentService.ProcessPaymentAsync(request);
        return StatusCode(response.StatusCode, response);
    }

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