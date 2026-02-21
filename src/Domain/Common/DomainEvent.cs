namespace TaskManagement.Domain.Common;

public abstract class DomainEvent
{
    protected DomainEvent()
    {
        OccurredOn = DateTimeOffset.UtcNow;
    }

    public DateTimeOffset OccurredOn { get; }
}