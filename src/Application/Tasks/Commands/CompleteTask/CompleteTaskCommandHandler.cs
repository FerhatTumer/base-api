using MediatR;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Aggregates.ProjectAggregate;
using TaskManagement.Domain.Common;
using DomainTaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Application.Tasks.Commands.CompleteTask;

public sealed class CompleteTaskCommandHandler : IRequestHandler<CompleteTaskCommand, Result>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteTaskCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
    {
        Project? project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException(nameof(Project), request.ProjectId);
        }

        project.ChangeTaskStatus(request.TaskId, DomainTaskStatus.Done);
        _projectRepository.Update(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}