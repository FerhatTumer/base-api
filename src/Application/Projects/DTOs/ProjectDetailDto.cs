using TaskManagement.Application.Tasks.DTOs;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Projects.DTOs;

public record ProjectDetailDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public ProjectStatus Status { get; init; }

    public int OwnerId { get; init; }

    public int TaskCount { get; init; }

    public IReadOnlyCollection<TaskDto> Tasks { get; init; } = Array.Empty<TaskDto>();

    public DateTimeOffset CreatedAt { get; init; }

    public DateTimeOffset? UpdatedAt { get; init; }
}