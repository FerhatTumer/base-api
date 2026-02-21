using MediatR;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Aggregates.ProjectAggregate.Events;

namespace TaskManagement.Application.Projects.EventHandlers;

public sealed class ProjectArchivedEventHandler : INotificationHandler<DomainEventNotification<ProjectArchivedEvent>>
{
    private readonly ILogger<ProjectArchivedEventHandler> _logger;

    public ProjectArchivedEventHandler(ILogger<ProjectArchivedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<ProjectArchivedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Project archived. ProjectId: {ProjectId}", notification.DomainEvent.ProjectId);
        return Task.CompletedTask;
    }
}