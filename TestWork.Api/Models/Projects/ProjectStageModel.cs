namespace TestWork.Api.Models.Projects;

public class ProjectStageModel
{
    public Guid? Id { get; set; }
    public int Stage { get; set; }
    public string Name { get; set; } = null!;
}