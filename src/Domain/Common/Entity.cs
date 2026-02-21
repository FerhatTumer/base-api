namespace TaskManagement.Domain.Common;

public abstract class Entity<TId>
{
    protected Entity()
    {
        CreatedAt = DateTimeOffset.UtcNow;
        IsDeleted = false;
    }

    public TId Id { get; protected set; } = default!;

    public DateTimeOffset CreatedAt { get; protected set; }

    public DateTimeOffset? UpdatedAt { get; protected set; }

    public bool IsDeleted { get; protected set; }

    protected void SetUpdated()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public virtual void SoftDelete()
    {
        if (IsDeleted)
        {
            throw new DomainException("Entity is already deleted.");
        }

        IsDeleted = true;
        SetUpdated();
    }
}