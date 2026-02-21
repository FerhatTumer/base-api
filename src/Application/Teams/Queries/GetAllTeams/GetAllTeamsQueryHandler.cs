using AutoMapper;
using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Teams.DTOs;
using TaskManagement.Domain.Aggregates.TeamAggregate;

namespace TaskManagement.Application.Teams.Queries.GetAllTeams;

public sealed class GetAllTeamsQueryHandler : IRequestHandler<GetAllTeamsQuery, PaginatedList<TeamListDto>>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;

    public GetAllTeamsQueryHandler(ITeamRepository teamRepository, IMapper mapper)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TeamListDto>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Team> teams = await _teamRepository.GetAllAsync(cancellationToken);
        List<TeamListDto> mapped = _mapper.Map<List<TeamListDto>>(teams.OrderBy(x => x.Name).ToList());
        return await PaginatedList<TeamListDto>.CreateAsync(mapped.AsQueryable(), request.PageNumber, Math.Min(request.PageSize, 100), cancellationToken);
    }
}