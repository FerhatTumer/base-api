using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Aggregates.ProjectAggregate.Events;

public sealed class ProjectArchivedEvent : DomainEvent
{
    public ProjectArchivedEvent(int projectId, DateTimeOffset archivedAt)
    {
        ProjectId = projectId;
        ArchivedAt = archivedAt;
    }

    public int ProjectId { get; }

    public DateTimeOffset ArchivedAt { get; }
}