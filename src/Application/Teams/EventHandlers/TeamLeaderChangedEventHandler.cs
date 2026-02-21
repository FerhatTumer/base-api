using MediatR;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Aggregates.TeamAggregate.Events;

namespace TaskManagement.Application.Teams.EventHandlers;

public sealed class TeamLeaderChangedEventHandler : INotificationHandler<DomainEventNotification<TeamLeaderChangedEvent>>
{
    private readonly ILogger<TeamLeaderChangedEventHandler> _logger;

    public TeamLeaderChangedEventHandler(ILogger<TeamLeaderChangedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<TeamLeaderChangedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Team leader changed. TeamId: {TeamId}, OldLeaderId: {OldLeaderId}, NewLeaderId: {NewLeaderId}",
            notification.DomainEvent.TeamId,
            notification.DomainEvent.OldLeaderId,
            notification.DomainEvent.NewLeaderId);

        return Task.CompletedTask;
    }
}