using MediatR;
using TaskManagement.Application.Common.Models;

namespace TaskManagement.Application.Tasks.Commands.AssignTask;

public record AssignTaskCommand(int ProjectId, int TaskId, int AssigneeId) : IRequest<Result>;