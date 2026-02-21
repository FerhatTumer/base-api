using MediatR;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Aggregates.ProjectAggregate;
using TaskManagement.Domain.Common;

namespace TaskManagement.Application.Projects.Commands.ArchiveProject;

public sealed class ArchiveProjectCommandHandler : IRequestHandler<ArchiveProjectCommand, Result>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public ArchiveProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(ArchiveProjectCommand request, CancellationToken cancellationToken)
    {
        Project? project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException(nameof(Project), request.ProjectId);
        }

        if (_currentUserService.IsAuthenticated && _currentUserService.UserId.HasValue && _currentUserService.UserId.Value != project.OwnerId)
        {
            throw new ForbiddenAccessException();
        }

        project.Archive();
        _projectRepository.Update(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}