using TaskManagement.Domain.Aggregates.ProjectAggregate.Events;
using TaskManagement.Domain.Common;
using TaskManagement.Domain.Enums;
using DomainTaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Domain.Aggregates.ProjectAggregate;

public class Project : AggregateRoot<int>
{
    private const int MaxNameLength = 200;
    private const int MaxDescriptionLength = 2000;
    private const int MaxTasksPerProject = 100;

    private readonly List<TaskItem> _taskItems = new();

    private Project(string name, string? description, int ownerId, ProjectStatus status)
    {
        SetName(name);
        SetDescription(description);
        SetStatus(status);
        SetOwner(ownerId);
    }

    protected Project()
    {
        Name = string.Empty;
        Status = ProjectStatus.Active;
    }

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public int OwnerId { get; private set; }

    public ProjectStatus Status { get; private set; }

    public IReadOnlyCollection<TaskItem> TaskItems => _taskItems.AsReadOnly();

    public static Project Create(string name, string? description, int ownerId, ProjectStatus status = ProjectStatus.Active)
    {
        Project project = new(name, description, ownerId, status);
        project.AddDomainEvent(new ProjectCreatedEvent(project.Id, project.Name, project.CreatedAt));
        return project;
    }

    public TaskItem AddTask(string title, string? description, Priority priority, DateTimeOffset? dueDate, int? assigneeId, decimal? estimatedHours)
    {
        EnsureNotDeleted();

        if (Status == ProjectStatus.Archived)
        {
            throw new DomainException("Cannot add tasks to an archived project.");
        }

        if (_taskItems.Count >= MaxTasksPerProject)
        {
            throw new DomainException($"A project cannot have more than {MaxTasksPerProject} tasks.");
        }

        TaskItem taskItem = TaskItem.Create(this, title, description, priority, dueDate, assigneeId, estimatedHours);
        _taskItems.Add(taskItem);
        SetUpdated();
        AddDomainEvent(new TaskCreatedEvent(taskItem.Id, Id, taskItem.Title, DateTimeOffset.UtcNow));
        return taskItem;
    }

    public void UpdateDetails(string name, string? description)
    {
        EnsureNotDeleted();

        SetName(name);
        SetDescription(description);
        SetUpdated();
    }

    public void AssignTask(int taskId, int assigneeId)
    {
        EnsureNotDeleted();

        TaskItem taskItem = GetTaskOrThrow(taskId);
        taskItem.AssignTo(assigneeId);
        SetUpdated();
        AddDomainEvent(new TaskAssignedEvent(taskItem.Id, assigneeId, DateTimeOffset.UtcNow));
    }

    public void ChangeTaskStatus(int taskId, DomainTaskStatus newStatus)
    {
        EnsureNotDeleted();

        TaskItem taskItem = GetTaskOrThrow(taskId);
        DomainTaskStatus previousStatus = taskItem.Status;
        taskItem.ChangeStatus(newStatus);
        SetUpdated();

        if (previousStatus != DomainTaskStatus.Done && taskItem.Status == DomainTaskStatus.Done)
        {
            AddDomainEvent(new TaskCompletedEvent(taskItem.Id, DateTimeOffset.UtcNow));
        }
    }

    public void UpdateTaskDetails(int taskId, string title, string? description, Priority priority, DateTimeOffset? dueDate, decimal? estimatedHours)
    {
        EnsureNotDeleted();

        TaskItem taskItem = GetTaskOrThrow(taskId);
        taskItem.UpdateDetails(title, description, priority, dueDate, estimatedHours);
        SetUpdated();
    }

    public void Archive()
    {
        EnsureNotDeleted();

        if (Status == ProjectStatus.Archived)
        {
            return;
        }

        if (_taskItems.Any(task => task.Status is DomainTaskStatus.Todo or DomainTaskStatus.InProgress))
        {
            throw new DomainException("Cannot archive a project with active tasks.");
        }

        Status = ProjectStatus.Archived;
        SetUpdated();
        AddDomainEvent(new ProjectArchivedEvent(Id, DateTimeOffset.UtcNow));
    }

    private TaskItem GetTaskOrThrow(int taskId)
    {
        if (taskId <= 0)
        {
            throw new DomainException("TaskId must be greater than zero.");
        }

        TaskItem? taskItem = _taskItems.FirstOrDefault(task => task.Id == taskId);
        if (taskItem is null)
        {
            throw new DomainException("Task not found in project.");
        }

        return taskItem;
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Project name cannot be empty or whitespace.");
        }

        string trimmedName = name.Trim();
        if (trimmedName.Length > MaxNameLength)
        {
            throw new DomainException($"Project name cannot exceed {MaxNameLength} characters.");
        }

        Name = trimmedName;
    }

    private void SetDescription(string? description)
    {
        if (description is not null && description.Length > MaxDescriptionLength)
        {
            throw new DomainException($"Project description cannot exceed {MaxDescriptionLength} characters.");
        }

        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
    }

    private void SetOwner(int ownerId)
    {
        if (ownerId <= 0)
        {
            throw new DomainException("OwnerId must be greater than zero.");
        }

        OwnerId = ownerId;
    }

    private void SetStatus(ProjectStatus status)
    {
        if (!Enum.IsDefined(status))
        {
            throw new DomainException("Invalid project status.");
        }

        Status = status;
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot modify a deleted project.");
        }
    }
}