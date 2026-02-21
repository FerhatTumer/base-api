using AutoMapper;
using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Projects.DTOs;
using TaskManagement.Domain.Aggregates.ProjectAggregate;

namespace TaskManagement.Application.Projects.Queries.GetAllProjects;

public sealed class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, PaginatedList<ProjectListDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public GetAllProjectsQueryHandler(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProjectListDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Project> projects = await _projectRepository.GetAllAsync(cancellationToken);
        IQueryable<Project> queryable = projects.AsQueryable();

        queryable = (request.SortBy?.ToLowerInvariant(), request.Descending) switch
        {
            ("name", false) => queryable.OrderBy(x => x.Name),
            ("name", true) => queryable.OrderByDescending(x => x.Name),
            ("createdat", false) => queryable.OrderBy(x => x.CreatedAt),
            ("createdat", true) => queryable.OrderByDescending(x => x.CreatedAt),
            (_, false) => queryable.OrderBy(x => x.Id),
            _ => queryable.OrderByDescending(x => x.Id)
        };

        List<ProjectListDto> mapped = _mapper.Map<List<ProjectListDto>>(queryable.ToList());
        return await PaginatedList<ProjectListDto>.CreateAsync(mapped.AsQueryable(), request.PageNumber, Math.Min(request.PageSize, 100), cancellationToken);
    }
}