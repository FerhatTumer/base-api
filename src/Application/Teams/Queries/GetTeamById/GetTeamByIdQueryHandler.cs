using AutoMapper;
using MediatR;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.Teams.DTOs;
using TaskManagement.Domain.Aggregates.TeamAggregate;

namespace TaskManagement.Application.Teams.Queries.GetTeamById;

public sealed class GetTeamByIdQueryHandler : IRequestHandler<GetTeamByIdQuery, TeamDetailDto>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;

    public GetTeamByIdQueryHandler(ITeamRepository teamRepository, IMapper mapper)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
    }

    public async Task<TeamDetailDto> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
    {
        Team? team = await _teamRepository.GetByIdAsync(request.TeamId, cancellationToken);
        if (team is null)
        {
            throw new NotFoundException(nameof(Team), request.TeamId);
        }

        return _mapper.Map<TeamDetailDto>(team);
    }
}