using TaskManagement.Domain.Enums;
using DomainTaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Application.Tasks.DTOs;

public record TaskDetailDto
{
    public int Id { get; init; }

    public int ProjectId { get; init; }

    public string Title { get; init; } = string.Empty;

    public string? Description { get; init; }

    public DomainTaskStatus Status { get; init; }

    public Priority Priority { get; init; }

    public int? AssigneeId { get; init; }

    public DateTimeOffset? DueDate { get; init; }

    public decimal? EstimatedHours { get; init; }

    public DateTimeOffset CreatedAt { get; init; }

    public DateTimeOffset? UpdatedAt { get; init; }
}