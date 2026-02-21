namespace TaskManagement.Application.Teams.DTOs;

public record TeamDetailDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public int LeaderId { get; init; }

    public IReadOnlyCollection<TeamMemberDto> Members { get; init; } = Array.Empty<TeamMemberDto>();

    public DateTimeOffset CreatedAt { get; init; }

    public DateTimeOffset? UpdatedAt { get; init; }
}