using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Teams.DTOs;

namespace TaskManagement.Application.Teams.Queries.GetAllTeams;

public record GetAllTeamsQuery(int PageNumber = 1, int PageSize = 10)
    : IRequest<PaginatedList<TeamListDto>>;