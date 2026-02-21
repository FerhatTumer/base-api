using MediatR;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Aggregates.ProjectAggregate.Events;

namespace TaskManagement.Application.Projects.EventHandlers;

public sealed class ProjectCreatedEventHandler : INotificationHandler<DomainEventNotification<ProjectCreatedEvent>>
{
    private readonly ILogger<ProjectCreatedEventHandler> _logger;

    public ProjectCreatedEventHandler(ILogger<ProjectCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<ProjectCreatedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Project created. ProjectId: {ProjectId}, Name: {Name}", notification.DomainEvent.ProjectId, notification.DomainEvent.Name);
        return Task.CompletedTask;
    }
}