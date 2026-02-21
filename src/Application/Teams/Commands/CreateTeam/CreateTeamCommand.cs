using MediatR;
using TaskManagement.Application.Common.Models;

namespace TaskManagement.Application.Teams.Commands.CreateTeam;

public record CreateTeamCommand(string Name, string? Description, int LeaderId) : IRequest<Result<int>>;