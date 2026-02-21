using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Aggregates.ProjectAggregate.Events;

public sealed class TaskCompletedEvent : DomainEvent
{
    public TaskCompletedEvent(int taskId, DateTimeOffset completedAt)
    {
        TaskId = taskId;
        CompletedAt = completedAt;
    }

    public int TaskId { get; }

    public DateTimeOffset CompletedAt { get; }
}