namespace TestWork.ReadModels;

public record UserReadModel(
    Guid Id,
    string FirstName,
    string LastName,
    string? MiddleName,
    string Email,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);