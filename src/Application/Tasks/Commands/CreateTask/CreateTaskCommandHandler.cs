using MediatR;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Aggregates.ProjectAggregate;
using TaskManagement.Domain.Common;

namespace TaskManagement.Application.Tasks.Commands.CreateTask;

public sealed class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<int>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTaskCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        Project? project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException(nameof(Project), request.ProjectId);
        }

        TaskManagement.Domain.Aggregates.ProjectAggregate.TaskItem task = project.AddTask(
            request.Title,
            request.Description,
            request.Priority,
            request.DueDate,
            request.AssigneeId,
            request.EstimatedHours);

        _projectRepository.Update(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(task.Id);
    }
}