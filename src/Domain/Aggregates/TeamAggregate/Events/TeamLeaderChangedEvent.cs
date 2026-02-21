using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Aggregates.TeamAggregate.Events;

public sealed class TeamLeaderChangedEvent : DomainEvent
{
    public TeamLeaderChangedEvent(int teamId, int oldLeaderId, int newLeaderId, DateTimeOffset changedAt)
    {
        TeamId = teamId;
        OldLeaderId = oldLeaderId;
        NewLeaderId = newLeaderId;
        ChangedAt = changedAt;
    }

    public int TeamId { get; }

    public int OldLeaderId { get; }

    public int NewLeaderId { get; }

    public DateTimeOffset ChangedAt { get; }
}