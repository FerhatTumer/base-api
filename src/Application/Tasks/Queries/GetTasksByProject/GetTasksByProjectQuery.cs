using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Tasks.DTOs;

namespace TaskManagement.Application.Tasks.Queries.GetTasksByProject;

public record GetTasksByProjectQuery(int ProjectId, int PageNumber = 1, int PageSize = 10)
    : IRequest<PaginatedList<TaskListDto>>;