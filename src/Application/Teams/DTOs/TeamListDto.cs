namespace TaskManagement.Application.Teams.DTOs;

public record TeamListDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public int LeaderId { get; init; }

    public int MemberCount { get; init; }

    public DateTimeOffset CreatedAt { get; init; }
}