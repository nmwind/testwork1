using TestWork.Entities;
using TestWork.ReadModels;

namespace TestWork.Repositories;

public interface IProjectTasksRepository
{
    Task<IReadOnlyCollection<ProjectTask>> GetAsync(Guid projectId);
    
    Task<int> GetCountAsync(Guid projectId);

    Task<ProjectTask?> GetByIdAsync(Guid taskId);

    Task InsertAsync(ProjectTask task);

    Task UpdateAsync(ProjectTask task);
}