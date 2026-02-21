using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Teams.Commands.AddTeamMember;

public record AddTeamMemberCommand(int TeamId, int UserId, TeamRole Role) : IRequest<Result>;