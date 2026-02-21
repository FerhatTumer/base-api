using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Projects.DTOs;

public record ProjectDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public ProjectStatus Status { get; init; }

    public int OwnerId { get; init; }

    public DateTimeOffset CreatedAt { get; init; }
}