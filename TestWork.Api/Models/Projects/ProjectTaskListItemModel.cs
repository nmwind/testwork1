namespace TestWork.Api.Models.Tasks;

public class ProjectTaskListItemModel
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public int Stage { get; set; }
    public int Order { get; set; }
    public string Title { get; set; } = null!;
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}