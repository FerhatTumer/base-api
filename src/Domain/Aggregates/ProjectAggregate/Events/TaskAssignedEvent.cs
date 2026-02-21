using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Aggregates.ProjectAggregate.Events;

public sealed class TaskAssignedEvent : DomainEvent
{
    public TaskAssignedEvent(int taskId, int assigneeId, DateTimeOffset assignedAt)
    {
        TaskId = taskId;
        AssigneeId = assigneeId;
        AssignedAt = assignedAt;
    }

    public int TaskId { get; }

    public int AssigneeId { get; }

    public DateTimeOffset AssignedAt { get; }
}