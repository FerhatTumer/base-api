using AutoMapper;
using MediatR;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Tasks.DTOs;
using TaskManagement.Domain.Aggregates.ProjectAggregate;
using DomainTaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Application.Tasks.Queries.GetOverdueTasks;

public sealed class GetOverdueTasksQueryHandler : IRequestHandler<GetOverdueTasksQuery, PaginatedList<TaskListDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;

    public GetOverdueTasksQueryHandler(IProjectRepository projectRepository, IMapper mapper, IDateTimeProvider dateTimeProvider)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<PaginatedList<TaskListDto>> Handle(GetOverdueTasksQuery request, CancellationToken cancellationToken)
    {
        DateTimeOffset now = _dateTimeProvider.UtcNow;
        IEnumerable<Project> projects = await _projectRepository.GetAllAsync(cancellationToken);

        List<TaskListDto> mapped = _mapper.Map<List<TaskListDto>>(
            projects.SelectMany(x => x.TaskItems)
                .Where(x => x.DueDate.HasValue && x.DueDate.Value < now && x.Status is not DomainTaskStatus.Done and not DomainTaskStatus.Cancelled)
                .OrderBy(x => x.DueDate)
                .ToList());

        return await PaginatedList<TaskListDto>.CreateAsync(mapped.AsQueryable(), request.PageNumber, Math.Min(request.PageSize, 100), cancellationToken);
    }
}