using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Projects.DTOs;

namespace TaskManagement.Application.Projects.Queries.GetAllProjects;

public record GetAllProjectsQuery(int PageNumber = 1, int PageSize = 10, string? SortBy = null, bool Descending = false)
    : IRequest<PaginatedList<ProjectListDto>>;