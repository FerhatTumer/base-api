using MediatR;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Aggregates.ProjectAggregate;
using TaskManagement.Domain.Common;

namespace TaskManagement.Application.Projects.Commands.CreateProject;

public sealed class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<int>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CreateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<int>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.IsAuthenticated && _currentUserService.UserId.HasValue && _currentUserService.UserId.Value != request.OwnerId)
        {
            throw new ForbiddenAccessException();
        }

        Project project = Project.Create(request.Name, request.Description, request.OwnerId);
        await _projectRepository.AddAsync(project, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(project.Id);
    }
}