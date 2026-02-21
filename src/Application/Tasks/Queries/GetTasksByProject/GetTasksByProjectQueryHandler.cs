using AutoMapper;
using MediatR;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Tasks.DTOs;
using TaskManagement.Domain.Aggregates.ProjectAggregate;

namespace TaskManagement.Application.Tasks.Queries.GetTasksByProject;

public sealed class GetTasksByProjectQueryHandler : IRequestHandler<GetTasksByProjectQuery, PaginatedList<TaskListDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public GetTasksByProjectQueryHandler(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TaskListDto>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
    {
        Project? project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException(nameof(Project), request.ProjectId);
        }

        List<TaskListDto> mapped = _mapper.Map<List<TaskListDto>>(project.TaskItems.OrderBy(x => x.Id).ToList());
        return await PaginatedList<TaskListDto>.CreateAsync(mapped.AsQueryable(), request.PageNumber, Math.Min(request.PageSize, 100), cancellationToken);
    }
}