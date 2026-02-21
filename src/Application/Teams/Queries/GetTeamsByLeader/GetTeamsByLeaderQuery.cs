using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Teams.DTOs;

namespace TaskManagement.Application.Teams.Queries.GetTeamsByLeader;

public record GetTeamsByLeaderQuery(int LeaderId, int PageNumber = 1, int PageSize = 10)
    : IRequest<PaginatedList<TeamListDto>>;