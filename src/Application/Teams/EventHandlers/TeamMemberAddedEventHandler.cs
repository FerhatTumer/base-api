using MediatR;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Aggregates.TeamAggregate.Events;

namespace TaskManagement.Application.Teams.EventHandlers;

public sealed class TeamMemberAddedEventHandler : INotificationHandler<DomainEventNotification<TeamMemberAddedEvent>>
{
    private readonly ILogger<TeamMemberAddedEventHandler> _logger;

    public TeamMemberAddedEventHandler(ILogger<TeamMemberAddedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<TeamMemberAddedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Team member added. TeamId: {TeamId}, UserId: {UserId}, Role: {Role}",
            notification.DomainEvent.TeamId,
            notification.DomainEvent.UserId,
            notification.DomainEvent.Role);

        return Task.CompletedTask;
    }
}