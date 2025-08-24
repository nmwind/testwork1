using TestWork.Entities;
using TestWork.ReadModels;

namespace TestWork.Repositories;

public interface IUsersRepository
{
    Task<PagedResult<UserReadModel>> GetAsync(string? searchValue,
        string? orderBy,
        bool ascending,
        int pageIndex,
        int pageSize);

    Task<User?> GetByIdAsync(Guid userId);

    Task<User?> GetByEmailAsync(string email);

    Task InsertAsync(User user);

    Task UpdateAsync(User user);
}