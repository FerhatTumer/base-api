using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Common;

namespace TaskManagement.Infrastructure.Persistence.Interceptors;

public sealed class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    public DispatchDomainEventsInterceptor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        DbContext? context = eventData.Context;
        if (context is null)
        {
            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        List<AggregateRoot<int>> aggregates = context.ChangeTracker
            .Entries<AggregateRoot<int>>()
            .Select(x => x.Entity)
            .Where(x => x.DomainEvents.Count > 0)
            .ToList();

        List<DomainEvent> domainEvents = aggregates
            .SelectMany(x => x.DomainEvents)
            .ToList();

        foreach (AggregateRoot<int> aggregate in aggregates)
        {
            aggregate.ClearDomainEvents();
        }

        foreach (DomainEvent domainEvent in domainEvents)
        {
            Type notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            object notification = Activator.CreateInstance(notificationType, domainEvent)!;
            await _mediator.Publish((INotification)notification, cancellationToken);
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}