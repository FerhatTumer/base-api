using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Aggregates.ProjectAggregate.Events;

public sealed class ProjectCreatedEvent : DomainEvent
{
    public ProjectCreatedEvent(int projectId, string name, DateTimeOffset createdAt)
    {
        ProjectId = projectId;
        Name = name;
        CreatedAt = createdAt;
    }

    public int ProjectId { get; }

    public string Name { get; }

    public DateTimeOffset CreatedAt { get; }
}