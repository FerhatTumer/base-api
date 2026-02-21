using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Aggregates.TeamAggregate;
using TaskManagement.Domain.Common;

namespace TaskManagement.Application.Teams.Commands.CreateTeam;

public sealed class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Result<int>>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTeamCommandHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        Team team = Team.Create(request.Name, request.Description, request.LeaderId);
        await _teamRepository.AddAsync(team, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(team.Id);
    }
}