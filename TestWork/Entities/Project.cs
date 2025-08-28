namespace TestWork.Entities;

public class Project
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Risks { get; private set; } = null!;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public Guid? SupervisorId { get; private set; }
    public Guid? ExecutorId { get; private set; }
    public ProjectStatus Status { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private List<string> _stages;
    public IReadOnlyList<string> Stages => _stages.AsReadOnly();
    private List<ProjectTask> _tasks;
    public IReadOnlyList<ProjectTask> Tasks => _tasks.AsReadOnly();

    public Project(
        Guid id,
        string name,
        string description,
        string risks,
        DateTime startDate,
        DateTime endDate,
        Guid? supervisorId,
        Guid? executorId,
        ProjectStatus status,
        bool isDeleted,
        DateTime createdAt,
        DateTime updatedAt,
        IReadOnlyList<string> stages,
        IReadOnlyList<ProjectTask> tasks)
    {
        Id = id;
        Name = name;
        Description = description;
        Risks = risks;
        StartDate = startDate;
        EndDate = endDate;
        SupervisorId = supervisorId;
        ExecutorId = executorId;
        Status = status;
        IsDeleted = isDeleted;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        _stages = stages.ToList();
        _tasks = tasks.ToList();
    }

    public static Project Create(string name,
        string description,
        string risks,
        DateTime startDate,
        DateTime endDate,
        Guid? supervisorId,
        Guid? executorId)
    {
        var created = DateTime.UtcNow;
        return new Project(
            Guid.NewGuid(),
            name,
            description,
            risks,
            startDate,
            endDate,
            supervisorId,
            executorId,
            ProjectStatus.PreProject,
            false,
            created,
            created,
            new List<string>(),
            new List<ProjectTask>());
    }

    public void Update(
        Guid? supervisorId = null,
        Guid? executorId = null,
        ProjectStatus? status = null)
    {
        if (supervisorId.HasValue)
            SupervisorId = supervisorId;

        if (executorId.HasValue)
            ExecutorId = executorId;

        if (status.HasValue)
            Status = status.Value;

        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStages(IReadOnlyCollection<string> stages)
    {
        _stages.Clear();
        _stages.AddRange(stages);

        UpdatedAt = DateTime.UtcNow;
    }

    
    public void SetTasks(IReadOnlyCollection<ProjectTask> tasks)
    {
        _tasks.Clear();
        _tasks.AddRange(tasks);
        
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void AddTask(ProjectTask task)
    {
        var exist = GetTaskById(task.Id);
        if (exist == null)
        {
            _tasks.Add(task);
            
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void DeleteTask(Guid taskId)
    {
        var task = GetTaskById(taskId);
        if (task != null)
        {
            _tasks.Remove(task);
            
            UpdatedAt = DateTime.UtcNow;
        }
    }
    
    public ProjectTask? GetTaskById(Guid taskId)
    {
        return _tasks.FirstOrDefault(o => o.Id == taskId);
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