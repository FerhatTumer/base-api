using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TaskManagement.Domain.Aggregates.ProjectAggregate;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Persistence;
using TaskManagement.Infrastructure.Persistence.Repositories;
using Xunit;

namespace TaskManagement.Infrastructure.IntegrationTests;

public sealed class ProjectRepositoryTests
{
    [Fact]
    public async Task AddAndGetById_WithInMemoryDatabase_ReturnsStoredProject()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using ApplicationDbContext context = new(options, new NoOpMediator());
        ProjectRepository repository = new(context);
        Project project = Project.Create("Infra Project", "desc", 12);
        project.AddTask("Task", "desc", Priority.Medium, DateTimeOffset.UtcNow.AddDays(1), 12, 1.25m);

        await repository.AddAsync(project, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);

        Project? fetched = await repository.GetByIdAsync(project.Id, CancellationToken.None);

        fetched.Should().NotBeNull();
        fetched!.Name.Should().Be("Infra Project");
        fetched.TaskItems.Should().HaveCount(1);
    }

    private sealed class NoOpMediator : IMediator
    {
        public Task Publish(object notification, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
            => Task.CompletedTask;

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest
            => throw new NotImplementedException();

        public Task<object?> Send(object request, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();
    }
}
