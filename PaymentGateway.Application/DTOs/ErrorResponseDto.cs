namespace PaymentGateway.Application.DTOs;

public record ErrorResponseDto(
    string Message,
    int StatusCode,
    DateTime Timestamp = default)
{
    public ErrorResponseDto() : this(string.Empty, 400) { }
    public ErrorResponseDto(string message, int statusCode) 
        : this(message, statusCode, DateTime.UtcNow) { }
}