using AutoMapper;
using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Projects.DTOs;
using TaskManagement.Domain.Aggregates.ProjectAggregate;

namespace TaskManagement.Application.Projects.Queries.GetProjectsByOwner;

public sealed class GetProjectsByOwnerQueryHandler : IRequestHandler<GetProjectsByOwnerQuery, PaginatedList<ProjectListDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public GetProjectsByOwnerQueryHandler(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProjectListDto>> Handle(GetProjectsByOwnerQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Project> projects = await _projectRepository.WhereAsync(x => x.OwnerId == request.OwnerId, cancellationToken);
        List<ProjectListDto> mapped = _mapper.Map<List<ProjectListDto>>(projects.OrderByDescending(x => x.CreatedAt).ToList());
        return await PaginatedList<ProjectListDto>.CreateAsync(mapped.AsQueryable(), request.PageNumber, Math.Min(request.PageSize, 100), cancellationToken);
    }
}