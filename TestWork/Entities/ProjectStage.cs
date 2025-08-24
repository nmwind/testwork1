namespace TestWork.Entities;

public class ProjectStage(
    Guid id,
    Guid projectId,
    int stage,
    string title)
{
    public Guid Id { get; set; } = id;
    public Guid ProjectId { get; set; } = projectId;
    public int Stage { get; set; } = stage;
    public string Title { get; set; } = title;

    public static ProjectStage Create(
        Guid projectId,
        int stage,
        string title)
    {
        return new ProjectStage(Guid.NewGuid(), projectId, stage, title);
    }

    public void Update(int state, string title)
    {
        this.Stage = stage;
        this.Title = title;
    }
}