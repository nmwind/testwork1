using TestWork.Entities;

namespace TestWork.ReadModels;

public class ProjectReadModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid? SupervisorId { get; set; }
    public Guid? ExecutorId { get; set; }
    public ProjectStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    public int StagesCount { get; set; }
    public int TasksCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}