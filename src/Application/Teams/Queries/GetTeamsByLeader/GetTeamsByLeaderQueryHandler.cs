using AutoMapper;
using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Teams.DTOs;
using TaskManagement.Domain.Aggregates.TeamAggregate;

namespace TaskManagement.Application.Teams.Queries.GetTeamsByLeader;

public sealed class GetTeamsByLeaderQueryHandler : IRequestHandler<GetTeamsByLeaderQuery, PaginatedList<TeamListDto>>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;

    public GetTeamsByLeaderQueryHandler(ITeamRepository teamRepository, IMapper mapper)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TeamListDto>> Handle(GetTeamsByLeaderQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Team> teams = await _teamRepository.WhereAsync(x => x.LeaderId == request.LeaderId, cancellationToken);
        List<TeamListDto> mapped = _mapper.Map<List<TeamListDto>>(teams.OrderBy(x => x.Name).ToList());
        return await PaginatedList<TeamListDto>.CreateAsync(mapped.AsQueryable(), request.PageNumber, Math.Min(request.PageSize, 100), cancellationToken);
    }
}