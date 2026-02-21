using MediatR;
using TaskManagement.Application.Teams.DTOs;

namespace TaskManagement.Application.Teams.Queries.GetTeamById;

public record GetTeamByIdQuery(int TeamId) : IRequest<TeamDetailDto>;