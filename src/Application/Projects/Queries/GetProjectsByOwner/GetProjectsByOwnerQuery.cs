using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Projects.DTOs;

namespace TaskManagement.Application.Projects.Queries.GetProjectsByOwner;

public record GetProjectsByOwnerQuery(int OwnerId, int PageNumber = 1, int PageSize = 10)
    : IRequest<PaginatedList<ProjectListDto>>;