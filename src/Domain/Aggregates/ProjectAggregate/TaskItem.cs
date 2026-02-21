using TaskManagement.Domain.Common;
using TaskManagement.Domain.Enums;
using DomainTaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Domain.Aggregates.ProjectAggregate;

public class TaskItem : Entity<int>
{
    private const int MaxTitleLength = 200;
    private const int MaxDescriptionLength = 2000;

    private TaskItem(Project project, string title, string? description, Priority priority, DateTimeOffset? dueDate, int? assigneeId, decimal? estimatedHours)
    {
        Project = project;
        ProjectId = project.Id;
        SetTitle(title);
        SetDescription(description);
        SetPriority(priority);
        SetDueDate(dueDate);
        SetEstimatedHours(estimatedHours);
        AssigneeId = assigneeId;
        Status = DomainTaskStatus.Todo;
    }

    protected TaskItem()
    {
        Project = null!;
        Title = string.Empty;
        Status = DomainTaskStatus.Todo;
    }

    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public Priority Priority { get; private set; }

    public DomainTaskStatus Status { get; private set; }

    public DateTimeOffset? DueDate { get; private set; }

    public int? AssigneeId { get; private set; }

    public int ProjectId { get; private set; }

    public decimal? EstimatedHours { get; private set; }

    public Project Project { get; private set; }

    public static TaskItem Create(Project project, string title, string? description, Priority priority, DateTimeOffset? dueDate, int? assigneeId, decimal? estimatedHours)
    {
        if (project is null)
        {
            throw new DomainException("Project is required.");
        }

        return new TaskItem(project, title, description, priority, dueDate, assigneeId, estimatedHours);
    }

    internal void AssignTo(int assigneeId)
    {
        EnsureNotDeleted();

        if (assigneeId <= 0)
        {
            throw new DomainException("AssigneeId must be greater than zero.");
        }

        if (Status is DomainTaskStatus.Done or DomainTaskStatus.Cancelled)
        {
            throw new DomainException("Cannot assign a completed or cancelled task.");
        }

        AssigneeId = assigneeId;
        SetUpdated();
    }

    internal void ChangeStatus(DomainTaskStatus newStatus)
    {
        EnsureNotDeleted();
        EnsureValidTaskStatus(newStatus);

        if (newStatus == Status)
        {
            return;
        }

        if (newStatus == DomainTaskStatus.Done && AssigneeId is null)
        {
            throw new DomainException("Cannot mark task as completed without an assignee.");
        }

        if (!IsValidStatusTransition(Status, newStatus))
        {
            throw new DomainException($"Invalid task status transition from {Status} to {newStatus}.");
        }

        Status = newStatus;
        SetUpdated();
    }

    internal void UpdateDetails(string title, string? description, Priority priority, DateTimeOffset? dueDate, decimal? estimatedHours)
    {
        EnsureNotDeleted();

        SetTitle(title);
        SetDescription(description);
        SetPriority(priority);
        SetDueDate(dueDate);
        SetEstimatedHours(estimatedHours);
        SetUpdated();
    }

    private static bool IsValidStatusTransition(DomainTaskStatus currentStatus, DomainTaskStatus newStatus)
    {
        return currentStatus switch
        {
            DomainTaskStatus.Todo => newStatus is DomainTaskStatus.InProgress or DomainTaskStatus.Cancelled,
            DomainTaskStatus.InProgress => newStatus is DomainTaskStatus.Done or DomainTaskStatus.Cancelled,
            DomainTaskStatus.Done => false,
            DomainTaskStatus.Cancelled => false,
            _ => false
        };
    }

    private void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("Task title cannot be empty.");
        }

        string trimmedTitle = title.Trim();
        if (trimmedTitle.Length > MaxTitleLength)
        {
            throw new DomainException($"Task title cannot exceed {MaxTitleLength} characters.");
        }

        Title = trimmedTitle;
    }

    private void SetDescription(string? description)
    {
        if (description is not null && description.Length > MaxDescriptionLength)
        {
            throw new DomainException($"Task description cannot exceed {MaxDescriptionLength} characters.");
        }

        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
    }

    private void SetPriority(Priority priority)
    {
        if (!Enum.IsDefined(priority))
        {
            throw new DomainException("Invalid task priority.");
        }

        Priority = priority;
    }

    private void SetDueDate(DateTimeOffset? dueDate)
    {
        if (dueDate.HasValue && dueDate.Value <= DateTimeOffset.UtcNow)
        {
            throw new DomainException("Due date must be in the future.");
        }

        DueDate = dueDate;
    }

    private void SetEstimatedHours(decimal? estimatedHours)
    {
        if (estimatedHours.HasValue && estimatedHours.Value <= 0)
        {
            throw new DomainException("Estimated hours must be positive when set.");
        }

        EstimatedHours = estimatedHours;
    }

    private static void EnsureValidTaskStatus(DomainTaskStatus status)
    {
        if (!Enum.IsDefined(status))
        {
            throw new DomainException("Invalid task status.");
        }
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot modify a deleted task.");
        }
    }
}