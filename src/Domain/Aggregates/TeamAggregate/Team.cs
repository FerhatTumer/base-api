using TaskManagement.Domain.Aggregates.TeamAggregate.Events;
using TaskManagement.Domain.Common;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Aggregates.TeamAggregate;

public class Team : AggregateRoot<int>
{
    private const int MaxNameLength = 100;
    private const int MaxDescriptionLength = 1000;
    private const int MaxMembersPerTeam = 50;

    private readonly List<TeamMember> _members = new();

    private Team(string name, string? description, int leaderId)
    {
        SetName(name);
        SetDescription(description);

        if (leaderId <= 0)
        {
            throw new DomainException("LeaderId must be greater than zero.");
        }

        LeaderId = leaderId;
    }

    protected Team()
    {
        Name = string.Empty;
    }

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public int LeaderId { get; private set; }

    public IReadOnlyCollection<TeamMember> Members => _members.AsReadOnly();

    public static Team Create(string name, string? description, int leaderId)
    {
        Team team = new(name, description, leaderId);
        TeamMember leaderMember = TeamMember.Create(team, leaderId, TeamRole.Leader);
        team._members.Add(leaderMember);
        team.AddDomainEvent(new TeamMemberAddedEvent(team.Id, leaderId, TeamRole.Leader, DateTimeOffset.UtcNow));
        return team;
    }

    public TeamMember AddMember(int userId, TeamRole role)
    {
        EnsureNotDeleted();

        if (userId <= 0)
        {
            throw new DomainException("UserId must be greater than zero.");
        }

        if (!Enum.IsDefined(role))
        {
            throw new DomainException("Invalid team role.");
        }

        if (_members.Count >= MaxMembersPerTeam)
        {
            throw new DomainException($"A team cannot have more than {MaxMembersPerTeam} members.");
        }

        if (_members.Any(member => member.UserId == userId && !member.IsDeleted))
        {
            throw new DomainException("The same user cannot be added twice to the same team.");
        }

        if (role == TeamRole.Leader)
        {
            throw new DomainException("Use ChangeLeader to assign team leader.");
        }

        TeamMember member = TeamMember.Create(this, userId, role);
        _members.Add(member);
        SetUpdated();
        AddDomainEvent(new TeamMemberAddedEvent(Id, userId, role, DateTimeOffset.UtcNow));
        return member;
    }

    public void ChangeLeader(int newLeaderId)
    {
        EnsureNotDeleted();

        if (newLeaderId <= 0)
        {
            throw new DomainException("New leader id must be greater than zero.");
        }

        if (newLeaderId == LeaderId)
        {
            return;
        }

        TeamMember? newLeader = _members.FirstOrDefault(member => member.UserId == newLeaderId && !member.IsDeleted);
        if (newLeader is null)
        {
            throw new DomainException("Leader must be a team member.");
        }

        TeamMember currentLeader = _members.First(member => member.UserId == LeaderId && !member.IsDeleted);
        currentLeader.ChangeRole(TeamRole.Member);
        newLeader.ChangeRole(TeamRole.Leader);

        int oldLeaderId = LeaderId;
        LeaderId = newLeaderId;
        SetUpdated();
        AddDomainEvent(new TeamLeaderChangedEvent(Id, oldLeaderId, newLeaderId, DateTimeOffset.UtcNow));
    }

    public void RemoveMember(int userId, int? newLeaderId = null)
    {
        EnsureNotDeleted();

        TeamMember? member = _members.FirstOrDefault(m => m.UserId == userId && !m.IsDeleted);
        if (member is null)
        {
            throw new DomainException("Team member not found.");
        }

        if (member.UserId == LeaderId)
        {
            if (!newLeaderId.HasValue)
            {
                throw new DomainException("Cannot remove the team leader without assigning a new one.");
            }

            if (newLeaderId.Value == LeaderId)
            {
                throw new DomainException("New leader must be different from current leader.");
            }

            ChangeLeader(newLeaderId.Value);
        }

        member.SoftDelete();
        SetUpdated();
    }

    public void ChangeMemberRole(int actorUserId, int targetUserId, TeamRole newRole)
    {
        EnsureNotDeleted();

        if (actorUserId != LeaderId)
        {
            throw new DomainException("Role can only be changed by team leader.");
        }

        if (!Enum.IsDefined(newRole))
        {
            throw new DomainException("Invalid team role.");
        }

        TeamMember? targetMember = _members.FirstOrDefault(member => member.UserId == targetUserId && !member.IsDeleted);
        if (targetMember is null)
        {
            throw new DomainException("Team member not found.");
        }

        if (targetMember.UserId == LeaderId && newRole != TeamRole.Leader)
        {
            throw new DomainException("Cannot remove the team leader without assigning a new one.");
        }

        if (newRole == TeamRole.Leader)
        {
            ChangeLeader(targetUserId);
            return;
        }

        targetMember.ChangeRole(newRole);
        SetUpdated();
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Team name cannot be empty or whitespace.");
        }

        string trimmedName = name.Trim();
        if (trimmedName.Length > MaxNameLength)
        {
            throw new DomainException($"Team name cannot exceed {MaxNameLength} characters.");
        }

        Name = trimmedName;
    }

    private void SetDescription(string? description)
    {
        if (description is not null && description.Length > MaxDescriptionLength)
        {
            throw new DomainException($"Team description cannot exceed {MaxDescriptionLength} characters.");
        }

        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot modify a deleted team.");
        }
    }
}