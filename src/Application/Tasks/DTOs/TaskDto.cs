using TaskManagement.Domain.Enums;
using DomainTaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Application.Tasks.DTOs;

public record TaskDto
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public DomainTaskStatus Status { get; init; }

    public Priority Priority { get; init; }

    public int? AssigneeId { get; init; }

    public DateTimeOffset? DueDate { get; init; }
}