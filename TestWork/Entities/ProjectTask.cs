namespace TestWork.Entities;

public class ProjectTask
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public int Stage { get; set; }
    public int Order { get; set; }
    public string Title { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ProjectTask(
        Guid id,
        Guid projectId,
        int stage,
        int order,
        string title,
        DateOnly start,
        DateOnly end,
        bool isDeleted,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        ProjectId = projectId;
        Stage = stage;
        Order = order;
        Title = title;
        Start = start;
        End = end;
        IsDeleted = isDeleted;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static ProjectTask Create(Guid projectId, int stage, int order, string title, DateOnly start, DateOnly end)
    {
        var created = DateTime.UtcNow;
        return new ProjectTask(Guid.NewGuid(), projectId, stage, order, title, start, end, false, created, created);
    }

    public void Update(int stage, int order, string title, DateOnly start, DateOnly end)
    {
        Stage = stage;
        Order = order;
        Title = title;
        Start = start;
        End = end;
        UpdatedAt = DateTime.Now;
    }

    public virtual bool Delete()
    {
        if (!IsDeleted)
        {
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;


            return true;
        }

        return false;
    }

    public bool Restore()
    {
        if (IsDeleted)
        {
            IsDeleted = false;
            UpdatedAt = DateTime.UtcNow;


            return true;
        }

        return false;
    }
}