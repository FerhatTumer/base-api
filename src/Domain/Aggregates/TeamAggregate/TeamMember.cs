using TaskManagement.Domain.Common;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Aggregates.TeamAggregate;

public class TeamMember : Entity<int>
{
    private TeamMember(Team team, int userId, TeamRole role)
    {
        Team = team;
        TeamId = team.Id;

        if (userId <= 0)
        {
            throw new DomainException("UserId must be greater than zero.");
        }

        if (!Enum.IsDefined(role))
        {
            throw new DomainException("Invalid team role.");
        }

        UserId = userId;
        Role = role;
        JoinedAt = DateTimeOffset.UtcNow;
    }

    protected TeamMember()
    {
        Team = null!;
    }

    public int UserId { get; private set; }

    public TeamRole Role { get; private set; }

    public DateTimeOffset JoinedAt { get; private set; }

    public int TeamId { get; private set; }

    public Team Team { get; private set; }

    public static TeamMember Create(Team team, int userId, TeamRole role)
    {
        if (team is null)
        {
            throw new DomainException("Team is required.");
        }

        return new TeamMember(team, userId, role);
    }

    internal void ChangeRole(TeamRole role)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot change role for a deleted team member.");
        }

        if (!Enum.IsDefined(role))
        {
            throw new DomainException("Invalid team role.");
        }

        Role = role;
        SetUpdated();
    }
}