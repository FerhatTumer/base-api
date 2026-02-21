using AutoMapper;
using MediatR;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.Tasks.DTOs;
using TaskManagement.Domain.Aggregates.ProjectAggregate;

namespace TaskManagement.Application.Tasks.Queries.GetTaskById;

public sealed class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDetailDto>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public GetTaskByIdQueryHandler(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<TaskDetailDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Project> projects = await _projectRepository.GetAllAsync(cancellationToken);
        TaskManagement.Domain.Aggregates.ProjectAggregate.TaskItem? task = projects
            .SelectMany(x => x.TaskItems)
            .FirstOrDefault(t => t.Id == request.TaskId);

        if (task is null)
        {
            throw new NotFoundException("TaskItem", request.TaskId);
        }

        return _mapper.Map<TaskDetailDto>(task);
    }
}