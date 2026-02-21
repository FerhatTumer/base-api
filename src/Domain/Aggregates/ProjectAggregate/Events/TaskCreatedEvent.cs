using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Aggregates.ProjectAggregate.Events;

public sealed class TaskCreatedEvent : DomainEvent
{
    public TaskCreatedEvent(int taskId, int projectId, string title, DateTimeOffset createdAt)
    {
        TaskId = taskId;
        ProjectId = projectId;
        Title = title;
        CreatedAt = createdAt;
    }

    public int TaskId { get; }

    public int ProjectId { get; }

    public string Title { get; }

    public DateTimeOffset CreatedAt { get; }
}