using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Tasks.DTOs;

namespace TaskManagement.Application.Tasks.Queries.GetTasksByAssignee;

public record GetTasksByAssigneeQuery(int AssigneeId, int PageNumber = 1, int PageSize = 10)
    : IRequest<PaginatedList<TaskListDto>>;