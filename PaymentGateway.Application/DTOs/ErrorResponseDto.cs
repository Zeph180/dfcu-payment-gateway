namespace PaymentGateway.Application.DTOs;

/// <summary>
/// Represents a standardized structure for error responses.
/// </summary>
/// <param name="Message">The error message to return to the client.</param>
/// <param name="StatusCode">The HTTP status code associated with the error.</param>
/// <param name="Timestamp">The time the error occurred (UTC).</param>
public record ErrorResponseDto(
    string Message,
    int StatusCode,
    DateTime Timestamp = default)
{
    /// <summary>
    /// Initializes a default error response with a 400 Bad Request status and empty message.
    /// </summary>
    public ErrorResponseDto() : this(string.Empty, 400) { }
    
    /// <summary>
    /// Initializes an error response with a message and status code. Timestamp is set to current UTC time.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="statusCode">The HTTP status code.</param>
    public ErrorResponseDto(string message, int statusCode) 
        : this(message, statusCode, DateTime.UtcNow) { }
}