using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Enums;
using DomainTaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Application.Tasks.Commands.UpdateTaskStatus;

public record UpdateTaskStatusCommand(int ProjectId, int TaskId, DomainTaskStatus Status) : IRequest<Result>;