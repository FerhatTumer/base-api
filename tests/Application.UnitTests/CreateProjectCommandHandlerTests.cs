using FluentAssertions;
using Moq;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Projects.Commands.CreateProject;
using TaskManagement.Domain.Aggregates.ProjectAggregate;
using TaskManagement.Domain.Common;
using Xunit;

namespace TaskManagement.Application.UnitTests;

public sealed class CreateProjectCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenAuthenticatedUserDiffersFromOwner_ThrowsForbiddenAccessException()
    {
        Mock<IProjectRepository> projectRepository = new();
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ICurrentUserService> currentUserService = new();
        currentUserService.SetupGet(x => x.IsAuthenticated).Returns(true);
        currentUserService.SetupGet(x => x.UserId).Returns(999);

        CreateProjectCommandHandler handler = new(projectRepository.Object, unitOfWork.Object, currentUserService.Object);
        CreateProjectCommand command = new("Project Name", "Description", 123);

        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
        projectRepository.Verify(x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Never);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithValidRequest_AddsProjectAndSaves()
    {
        Mock<IProjectRepository> projectRepository = new();
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ICurrentUserService> currentUserService = new();
        currentUserService.SetupGet(x => x.IsAuthenticated).Returns(false);

        CreateProjectCommandHandler handler = new(projectRepository.Object, unitOfWork.Object, currentUserService.Object);
        CreateProjectCommand command = new("Project Name", "Description", 123);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        projectRepository.Verify(x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
