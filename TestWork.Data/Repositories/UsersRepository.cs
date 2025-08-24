using Microsoft.EntityFrameworkCore;
using TestWork.Data.Context;
using TestWork.Data.Entities;
using TestWork.Data.Extensions;
using TestWork.Entities;
using TestWork.ReadModels;
using TestWork.Repositories;


namespace TestWork.Data.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DbContextOptionsBuilder<DatabaseContext> _contextBuilder;

        public UsersRepository(DbContextOptionsBuilder<DatabaseContext> contextBuilder)
        {
            _contextBuilder = contextBuilder;
        }

        public async Task<PagedResult<UserReadModel>> GetAsync(
            string? searchValue,
            string? orderBy,
            bool ascending,
            int pageIndex,
            int pageSize)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var query = context.Users
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(o => EF.Functions.ILike(o.FirstName,
                                             $"%{searchValue}%") ||
                                         EF.Functions.ILike(o.LastName,
                                             $"%{searchValue}%") ||
                                         EF.Functions.ILike(o.MiddleName!,
                                             $"%{searchValue}%") ||
                                         EF.Functions.ILike(o.Email,
                                             $"%{searchValue}%"));
            }

            var pagedResult = await query.ToPagedResultAsync(
                orderBy,
                nameof(User.FirstName),
                ascending,
                pageIndex,
                pageSize);

            var entities = pagedResult.Items
                .Select(entity =>
                    new UserReadModel(Id: entity.Id,
                        FirstName: entity.FirstName,
                        LastName: entity.LastName,
                        MiddleName: entity.MiddleName,
                        Email: entity.Email,
                        CreatedAt: entity.CreatedAt,
                        UpdatedAt: entity.UpdatedAt))
                .ToList();

            return new PagedResult<UserReadModel>(
                Items: entities.ToList(),
                Total: pagedResult.Total
            );
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var entity = await context.Users
                .AsNoTracking()
                .Where(o => o.Id == userId)
                .FirstOrDefaultAsync();

            return Map(entity);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var entity = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Email == email);

            return Map(entity);
        }

        public async Task InsertAsync(User user)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var entity = new UserEntity
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            context.Users.Add(entity);

            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var entity = await context.Users
                .FirstAsync(o => o.Id == user.Id);

            entity.FirstName = user.FirstName;
            entity.LastName = user.LastName;
            entity.MiddleName = user.MiddleName;
            entity.CreatedAt = user.CreatedAt;
            entity.UpdatedAt = user.UpdatedAt;

            await context.SaveChangesAsync();
        }

        private static User? Map(UserEntity? entity)
        {
            if (entity == null) return null;

            return new User(
                entity.Id,
                entity.FirstName,
                entity.LastName,
                entity.MiddleName,
                entity.Email,
                entity.PasswordHash,
                entity.CreatedAt.UtcDateTime,
                entity.UpdatedAt.UtcDateTime
            );
        }
    }
}