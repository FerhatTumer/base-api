using MediatR;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Aggregates.ProjectAggregate.Events;

namespace TaskManagement.Application.Tasks.EventHandlers;

public sealed class TaskAssignedEventHandler : INotificationHandler<DomainEventNotification<TaskAssignedEvent>>
{
    private readonly ILogger<TaskAssignedEventHandler> _logger;

    public TaskAssignedEventHandler(ILogger<TaskAssignedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<TaskAssignedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Task assigned. TaskId: {TaskId}, AssigneeId: {AssigneeId}",
            notification.DomainEvent.TaskId,
            notification.DomainEvent.AssigneeId);

        return Task.CompletedTask;
    }
}