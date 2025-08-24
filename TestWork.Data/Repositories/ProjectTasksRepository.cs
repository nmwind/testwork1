using Microsoft.EntityFrameworkCore;
using TestWork.Data.Context;
using TestWork.Data.Entities;
using TestWork.Data.Extensions;
using TestWork.Entities;
using TestWork.ReadModels;
using TestWork.Repositories;


namespace TestWork.Data.Repositories
{
    public class ProjectTasksRepository : IProjectTasksRepository
    {
        private readonly DbContextOptionsBuilder<DatabaseContext> _contextBuilder;

        public ProjectTasksRepository(DbContextOptionsBuilder<DatabaseContext> contextBuilder)
        {
            _contextBuilder = contextBuilder;
        }

        public async Task<IReadOnlyCollection<ProjectTask>> GetAsync(Guid projectId)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var query = context.Tasks
                .AsNoTracking()
                .Where(e=>e.ProjectId == projectId)
                .AsQueryable();
            
            return query.Select(e=>Map(e)!).ToList();
        }

        public async Task<int> GetCountAsync(Guid projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectTask?> GetByIdAsync(Guid taskId)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var entity = await context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == taskId);

            return Map(entity);
        }


        public async Task InsertAsync(ProjectTask task)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var entity = new ProjectTaskEntity
            {
                Id = task.Id,
                ProjectId = task.ProjectId,
                Title = task.Title,
                Stage = task.Stage,
                Order = task.Order,
                Start = task.Start,
                End = task.End,
                IsDeleted = task.IsDeleted,
                UpdatedAt = task.UpdatedAt,
                CreatedAt = task.CreatedAt,
            };

            context.Tasks.Add(entity);

            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProjectTask task)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var entity = await context.Tasks
                .FirstAsync(o => o.Id == task.Id);

            entity.ProjectId = task.ProjectId;
            entity.Title = task.Title;
            entity.Stage = task.Stage; 
            entity.Order = task.Order;
            entity.Start = task.Start;
            entity.End = task.End;
            entity.IsDeleted = task.IsDeleted;
            entity.CreatedAt = task.CreatedAt;
            entity.UpdatedAt = task.UpdatedAt;

            await context.SaveChangesAsync();
        }

        private static ProjectTask? Map(ProjectTaskEntity? entity)
        {
            if (entity == null) return null;

            return new ProjectTask(
                entity.Id,
                entity.ProjectId,
                entity.Stage,
                entity.Order,
                entity.Title,
                entity.Start,
                entity.End,
                entity.IsDeleted,
                entity.CreatedAt.UtcDateTime,
                entity.UpdatedAt.UtcDateTime
            );
        }
    }
}