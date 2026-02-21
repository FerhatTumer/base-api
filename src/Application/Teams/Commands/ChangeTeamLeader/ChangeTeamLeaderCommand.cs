using MediatR;
using TaskManagement.Application.Common.Models;

namespace TaskManagement.Application.Teams.Commands.ChangeTeamLeader;

public record ChangeTeamLeaderCommand(int TeamId, int NewLeaderId) : IRequest<Result>;