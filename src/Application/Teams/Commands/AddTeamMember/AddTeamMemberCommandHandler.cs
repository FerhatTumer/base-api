using MediatR;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Aggregates.TeamAggregate;
using TaskManagement.Domain.Common;

namespace TaskManagement.Application.Teams.Commands.AddTeamMember;

public sealed class AddTeamMemberCommandHandler : IRequestHandler<AddTeamMemberCommand, Result>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddTeamMemberCommandHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddTeamMemberCommand request, CancellationToken cancellationToken)
    {
        Team? team = await _teamRepository.GetByIdAsync(request.TeamId, cancellationToken);
        if (team is null)
        {
            throw new NotFoundException(nameof(Team), request.TeamId);
        }

        team.AddMember(request.UserId, request.Role);
        _teamRepository.Update(team);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}