using TestWork.Entities;
using TestWork.ReadModels;

namespace TestWork.Repositories;

public interface IProjectsRepository
{
    Task<PagedResult<Project>> GetAsync(
        string? name,
        ProjectStatus? status,
        Guid? supervisorId,
        string? orderBy,
        bool ascending,
        int pageIndex,
        int pageSize);

    Task<Project?> GetByIdAsync(Guid projectId);

    Task InsertAsync(Project project);

    Task UpdateAsync(Project project);
}