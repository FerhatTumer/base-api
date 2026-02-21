using MediatR;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Aggregates.TeamAggregate;
using TaskManagement.Domain.Common;

namespace TaskManagement.Application.Teams.Commands.RemoveTeamMember;

public sealed class RemoveTeamMemberCommandHandler : IRequestHandler<RemoveTeamMemberCommand, Result>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveTeamMemberCommandHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveTeamMemberCommand request, CancellationToken cancellationToken)
    {
        Team? team = await _teamRepository.GetByIdAsync(request.TeamId, cancellationToken);
        if (team is null)
        {
            throw new NotFoundException(nameof(Team), request.TeamId);
        }

        team.RemoveMember(request.UserId, request.NewLeaderId);
        _teamRepository.Update(team);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}