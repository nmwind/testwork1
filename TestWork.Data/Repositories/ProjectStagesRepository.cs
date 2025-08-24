using Microsoft.EntityFrameworkCore;
using TestWork.Data.Context;
using TestWork.Data.Entities;
using TestWork.Data.Extensions;
using TestWork.Entities;
using TestWork.ReadModels;
using TestWork.Repositories;


namespace TestWork.Data.Repositories
{
    public class ProjectStagesRepository : IProjectStagesRepository
    {
        private readonly DbContextOptionsBuilder<DatabaseContext> _contextBuilder;

        public ProjectStagesRepository(DbContextOptionsBuilder<DatabaseContext> contextBuilder)
        {
            _contextBuilder = contextBuilder;
        }


        public async Task<IReadOnlyCollection<ProjectStage>> GetAsync(Guid projectId)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var query = context.Stages
                .AsNoTracking()
                .Where(p => p.ProjectId == projectId)
                .AsQueryable();

            return query.Select(entity => Map(entity)!).ToList();
        }

        public Task<int> GetCountAsync(Guid projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectStage?> GetByIdAsync(Guid projectId)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var entity = await context.Stages
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == projectId);

            return Map(entity);
        }


        public async Task InsertAsync(ProjectStage stage)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var entity = new ProjectStageEntity
            {
                Id = stage.Id,
                ProjectId = stage.ProjectId,
                Stage = stage.Stage,
                Title = stage.Title,
            };

            context.Stages.Add(entity);

            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProjectStage stage)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var entity = await context.Stages
                .FirstAsync(o => o.Id == stage.Id);

            entity.ProjectId = stage.ProjectId;
            entity.Title = stage.Title;
            entity.Stage = stage.Stage;
            
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid projectStageId)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var entity = context.Stages.FirstOrDefault(s=>s.Id == projectStageId);
            context.Stages.Remove(entity!);
            
            await context.SaveChangesAsync();
        }

        private static ProjectStage? Map(ProjectStageEntity? entity)
        {
            if (entity == null) return null;

            return new ProjectStage(
                entity.Id,
                entity.ProjectId,
                entity.Stage,
                entity.Title
            );
        }
    }
}