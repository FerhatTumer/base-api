using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Aggregates.ProjectAggregate.ValueObjects;

public sealed class DateRange : ValueObject
{
    private DateRange(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        if (endDate <= startDate)
        {
            throw new DomainException("EndDate must be after StartDate.");
        }

        StartDate = startDate;
        EndDate = endDate;
    }

    public DateTimeOffset StartDate { get; }

    public DateTimeOffset EndDate { get; }

    public static DateRange Create(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        return new DateRange(startDate, endDate);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return StartDate;
        yield return EndDate;
    }
}