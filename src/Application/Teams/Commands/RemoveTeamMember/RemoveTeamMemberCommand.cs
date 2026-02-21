using MediatR;
using TaskManagement.Application.Common.Models;

namespace TaskManagement.Application.Teams.Commands.RemoveTeamMember;

public record RemoveTeamMemberCommand(int TeamId, int UserId, int? NewLeaderId = null) : IRequest<Result>;