using TaskManagement.Domain.Common;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Aggregates.TeamAggregate.Events;

public sealed class TeamMemberAddedEvent : DomainEvent
{
    public TeamMemberAddedEvent(int teamId, int userId, TeamRole role, DateTimeOffset addedAt)
    {
        TeamId = teamId;
        UserId = userId;
        Role = role;
        AddedAt = addedAt;
    }

    public int TeamId { get; }

    public int UserId { get; }

    public TeamRole Role { get; }

    public DateTimeOffset AddedAt { get; }
}