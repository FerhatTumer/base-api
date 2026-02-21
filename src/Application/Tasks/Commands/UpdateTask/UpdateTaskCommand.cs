using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand(
    int ProjectId,
    int TaskId,
    string Title,
    string? Description,
    Priority Priority,
    DateTimeOffset? DueDate,
    decimal? EstimatedHours) : IRequest<Result>;