using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Tasks.DTOs;

namespace TaskManagement.Application.Tasks.Queries.GetOverdueTasks;

public record GetOverdueTasksQuery(int PageNumber = 1, int PageSize = 10)
    : IRequest<PaginatedList<TaskListDto>>;