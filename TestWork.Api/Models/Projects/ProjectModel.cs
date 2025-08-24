using TestWork.Api.Models.Tasks;
using TestWork.Entities;

namespace TestWork.Api.Models.Projects;

public class ProjectModel
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } =  null!;
    public string Description { get; set; } = null!;
    public string Risks { get; set; }  = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid? SupervisorId { get; set; }
    public Guid? ExecutorId { get; set; }
    public ProjectStatus Status { get; set; }
    public required IReadOnlyCollection<ProjectStageModel> Stages { get; set; }
    public required IReadOnlyCollection<ProjectTaskModel> Tasks { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}