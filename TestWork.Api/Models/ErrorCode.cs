namespace TestWork.Api.Models;

/// <summary>
/// Specifies the API error codes.
/// </summary>
public enum ErrorCode
{
    UnexpectedError,
    InvalidParameters,
    NotFound,
    DomainError,
    InvalidStatus,
    EmailAlreadyExists,
    PhoneNumberAlreadyExists,
    UserNotRegistered,
    InvalidPassword
}
