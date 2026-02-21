using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Tasks.Commands.CreateTask;

public record CreateTaskCommand(
    int ProjectId,
    string Title,
    string? Description,
    Priority Priority,
    DateTimeOffset? DueDate,
    int? AssigneeId,
    decimal? EstimatedHours) : IRequest<Result<int>>;