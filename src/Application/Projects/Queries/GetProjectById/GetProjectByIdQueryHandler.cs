using AutoMapper;
using MediatR;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.Projects.DTOs;
using TaskManagement.Domain.Aggregates.ProjectAggregate;

namespace TaskManagement.Application.Projects.Queries.GetProjectById;

public sealed class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDetailDto>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public GetProjectByIdQueryHandler(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<ProjectDetailDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        Project? project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException(nameof(Project), request.ProjectId);
        }

        return _mapper.Map<ProjectDetailDto>(project);
    }
}