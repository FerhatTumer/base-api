namespace TaskManagement.Application.Teams.DTOs;

public record TeamDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public int LeaderId { get; init; }

    public DateTimeOffset CreatedAt { get; init; }
}