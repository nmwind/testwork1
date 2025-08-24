namespace TestWork;

public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Total
);