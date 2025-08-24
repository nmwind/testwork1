namespace TestWork.Api.Models.Projects;

public class ProjectTaskCreateOrUpdateModel
{
    public Guid? Id { get; set; }
    public int Stage { get; set; }
    public int Order { get; set; }
    public string Title { get; set; } = null!;
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
}