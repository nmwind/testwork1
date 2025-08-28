
namespace TestWork.Api.Models.Projects;

public class ProjectUpdateModel
{
    public Guid Id { get; set; } 
    public string Name { get; set; } = null!;
    public Guid? SupervisorId { get; set; }
    public Guid? ExecutorId { get; set; }
    public IReadOnlyCollection<string> Stages { get; set; } = null!;
    public IReadOnlyCollection<ProjectTaskCreateOrUpdateModel>? Tasks { get; set; }
}