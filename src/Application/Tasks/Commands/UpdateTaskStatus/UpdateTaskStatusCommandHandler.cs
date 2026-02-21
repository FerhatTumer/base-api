using MediatR;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Aggregates.ProjectAggregate;
using TaskManagement.Domain.Common;

namespace TaskManagement.Application.Tasks.Commands.UpdateTaskStatus;

public sealed class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, Result>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTaskStatusCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        Project? project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException(nameof(Project), request.ProjectId);
        }

        project.ChangeTaskStatus(request.TaskId, request.Status);
        _projectRepository.Update(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}