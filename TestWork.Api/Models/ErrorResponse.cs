namespace TestWork.Api.Models;

/// <summary>
/// Represents an error that occurred while calling API.
/// </summary>
public class ErrorResponse
{
    public ErrorResponse()
    {
    }

    internal ErrorResponse(ErrorCode errorCode)
    {
        Code = errorCode.ToString();
    }

    internal ErrorResponse(ErrorCode errorCode, string message)
    {
        Code = errorCode.ToString();
        Message = message;
    }

    /// <summary>
    /// The error code.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// The error details.
    /// </summary>
    public string? Message { get; set; }

    internal static ErrorResponse UnexpectedError()
        => new (ErrorCode.UnexpectedError);
}