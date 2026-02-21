using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskManagement.Domain.Common;

namespace TaskManagement.Infrastructure.Persistence.Interceptors;

public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? context = eventData.Context;
        if (context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        DateTimeOffset utcNow = DateTimeOffset.UtcNow;

        foreach (EntityEntry entry in context.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted))
        {
            if (entry.Entity is not null && IsDomainEntity(entry.Entity.GetType()))
            {
                entry.State = EntityState.Modified;
                entry.Property(nameof(Entity<int>.IsDeleted)).CurrentValue = true;
                entry.Property(nameof(Entity<int>.UpdatedAt)).CurrentValue = utcNow;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static bool IsDomainEntity(Type type)
    {
        Type? current = type;
        while (current is not null)
        {
            if (current.IsGenericType && current.GetGenericTypeDefinition() == typeof(Entity<>))
            {
                return true;
            }

            current = current.BaseType;
        }

        return false;
    }
}