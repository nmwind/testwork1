using Microsoft.EntityFrameworkCore;
using TestWork.Data.Context;
using TestWork.Data.Entities;
using TestWork.Data.Extensions;
using TestWork.Entities;
using TestWork.ReadModels;
using TestWork.Repositories;


namespace TestWork.Data.Repositories
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly DbContextOptionsBuilder<DatabaseContext> _contextBuilder;

        public ProjectsRepository(DbContextOptionsBuilder<DatabaseContext> contextBuilder)
        {
            _contextBuilder = contextBuilder;
        }


        public async Task<PagedResult<Project>> GetAsync(
            string? name,
            ProjectStatus? status,
            Guid? supervisorId,
            string? orderBy,
            bool ascending,
            int pageIndex,
            int pageSize)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var query = context.Projects
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(o => EF.Functions.ILike(o.Name, $"%{name}%"));
            }

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            if (supervisorId.HasValue)
            {
                query = query.Where(o => o.SupervisorId == supervisorId.Value);
            }

            var pagedResult = await query.ToPagedResultAsync(
                orderBy,
                nameof(Project.Name),
                ascending,
                pageIndex,
                pageSize);

            var entities = pagedResult.Items
                .Select(e => Map(e)!)
                .ToList();

            return new PagedResult<Project>(
                Items: entities,
                Total: pagedResult.Total
            );
        }

        public async Task<Project?> GetByIdAsync(Guid projectId)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var entity = await context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == projectId);

            return Map(entity);
        }


        public async Task InsertAsync(Project project)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var entity = new ProjectEntity
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Risks = project.Risks,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                SupervisorId = project.SupervisorId,
                ExecutorId = project.ExecutorId,
                Status = project.Status,
                IsDeleted = project.IsDeleted,
                UpdatedAt = project.UpdatedAt,
                CreatedAt = project.CreatedAt,
            };

            for (int stage = 0; stage < project.Stages.Count; stage++)
            {
                entity.Stages.Add(new ProjectStageEntity
                {
                    ProjectId = project.Id,
                    Stage = stage,
                    Title = project.Stages[stage]
                });
            }

            context.Projects.Add(entity);

            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Project project)
        {
            await using var context = new DatabaseContext(_contextBuilder.Options);

            var entity = await context.Projects
                .Include(o=>o.Stages)
                .FirstAsync(o => o.Id == project.Id);

            entity.Name = project.Name;
            entity.Description = project.Description;
            entity.Risks = project.Risks;
            entity.StartDate = project.StartDate;
            entity.EndDate = project.EndDate;
            entity.SupervisorId = project.SupervisorId;
            entity.ExecutorId = project.ExecutorId;
            entity.Status = project.Status;
            entity.IsDeleted = project.IsDeleted;
            entity.CreatedAt = project.CreatedAt;
            entity.UpdatedAt = project.UpdatedAt;
            
            entity.Stages = project.Stages.Select((stageName, i) => new ProjectStageEntity
            {
                ProjectId = project.Id,
                Stage = i,
                Title = stageName,
            }).ToList();

            await context.SaveChangesAsync();
        }

        private static Project? Map(ProjectEntity? entity)
        {
            if (entity == null) return null;

            return new Project(
                entity.Id,
                entity.Name,
                entity.Description,
                entity.Risks,
                entity.StartDate,
                entity.EndDate,
                entity.SupervisorId,
                entity.ExecutorId,
                entity.Status,
                entity.IsDeleted,
                entity.CreatedAt.UtcDateTime,
                entity.UpdatedAt.UtcDateTime,
                entity.Stages?
                    .OrderBy(stage => stage.Stage)
                    .Select(stage => stage.Title)
                    .ToList() ?? []
            );
        }
    }
}