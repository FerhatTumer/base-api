using MediatR;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Aggregates.ProjectAggregate.Events;

namespace TaskManagement.Application.Tasks.EventHandlers;

public sealed class TaskCompletedEventHandler : INotificationHandler<DomainEventNotification<TaskCompletedEvent>>
{
    private readonly ILogger<TaskCompletedEventHandler> _logger;

    public TaskCompletedEventHandler(ILogger<TaskCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<TaskCompletedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Task completed. TaskId: {TaskId}", notification.DomainEvent.TaskId);
        return Task.CompletedTask;
    }
}