namespace TestWork.Api.Models.Projects;

public class ProjectCreateModel
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Risks { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid? SupervisorId { get; set; }
    public Guid? ExecutorId { get; set; }
}