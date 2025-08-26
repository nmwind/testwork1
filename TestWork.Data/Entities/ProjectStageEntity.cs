namespace TestWork.Data.Entities;

public class ProjectStageEntity
{
    public Guid ProjectId { get; set; } 
    public int Stage { get; set; }
    public string Title { get; set; } = null!;
}