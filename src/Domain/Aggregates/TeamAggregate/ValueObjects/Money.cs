using TaskManagement.Domain.Common;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Aggregates.TeamAggregate.ValueObjects;

public sealed class Money : ValueObject
{
    private Money(decimal amount, Currency currency)
    {
        if (amount < 0)
        {
            throw new DomainException("Money amount cannot be negative.");
        }

        if (!Enum.IsDefined(currency))
        {
            throw new DomainException("Invalid currency.");
        }

        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }

    public Currency Currency { get; }

    public static Money Create(decimal amount, Currency currency)
    {
        return new Money(amount, currency);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Amount;
        yield return Currency;
    }
}