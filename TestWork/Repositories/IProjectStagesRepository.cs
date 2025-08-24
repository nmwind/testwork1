using TestWork.Entities;
using TestWork.ReadModels;

namespace TestWork.Repositories;

public interface IProjectStagesRepository
{
    Task<IReadOnlyCollection<ProjectStage>> GetAsync(Guid projectId);
    
    Task<int> GetCountAsync(Guid projectId);

    Task<ProjectStage?> GetByIdAsync(Guid projectStageId);

    Task InsertAsync(ProjectStage projectStage);

    Task UpdateAsync(ProjectStage projectStage);    
    
    Task DeleteAsync(Guid projectStageId);
}