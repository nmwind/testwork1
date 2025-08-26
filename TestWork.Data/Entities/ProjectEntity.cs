using TestWork.Entities;

namespace TestWork.Data.Entities;

public class ProjectEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Risks { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid? SupervisorId { get; set; }
    public Guid? ExecutorId { get; set; }
    public ProjectStatus Status { get; set; }
    public ICollection<ProjectStageEntity>? Stages { get; set; } 
    public ICollection<ProjectTaskEntity>? Tasks { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}