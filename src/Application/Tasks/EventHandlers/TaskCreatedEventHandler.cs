using MediatR;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Aggregates.ProjectAggregate.Events;

namespace TaskManagement.Application.Tasks.EventHandlers;

public sealed class TaskCreatedEventHandler : INotificationHandler<DomainEventNotification<TaskCreatedEvent>>
{
    private readonly ILogger<TaskCreatedEventHandler> _logger;

    public TaskCreatedEventHandler(ILogger<TaskCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<TaskCreatedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Task created. TaskId: {TaskId}, ProjectId: {ProjectId}, Title: {Title}",
            notification.DomainEvent.TaskId,
            notification.DomainEvent.ProjectId,
            notification.DomainEvent.Title);

        return Task.CompletedTask;
    }
}