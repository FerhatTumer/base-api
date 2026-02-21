using MediatR;
using TaskManagement.Application.Common.Models;

namespace TaskManagement.Application.Tasks.Commands.CompleteTask;

public record CompleteTaskCommand(int ProjectId, int TaskId) : IRequest<Result>;