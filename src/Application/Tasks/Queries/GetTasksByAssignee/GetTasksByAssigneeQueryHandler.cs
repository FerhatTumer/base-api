using AutoMapper;
using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Tasks.DTOs;
using TaskManagement.Domain.Aggregates.ProjectAggregate;

namespace TaskManagement.Application.Tasks.Queries.GetTasksByAssignee;

public sealed class GetTasksByAssigneeQueryHandler : IRequestHandler<GetTasksByAssigneeQuery, PaginatedList<TaskListDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public GetTasksByAssigneeQueryHandler(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TaskListDto>> Handle(GetTasksByAssigneeQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Project> projects = await _projectRepository.GetAllAsync(cancellationToken);
        List<TaskListDto> mapped = _mapper.Map<List<TaskListDto>>(
            projects.SelectMany(x => x.TaskItems)
                .Where(x => x.AssigneeId == request.AssigneeId)
                .OrderByDescending(x => x.CreatedAt)
                .ToList());

        return await PaginatedList<TaskListDto>.CreateAsync(mapped.AsQueryable(), request.PageNumber, Math.Min(request.PageSize, 100), cancellationToken);
    }
}