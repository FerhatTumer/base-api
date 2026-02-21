using System.Text.RegularExpressions;
using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Aggregates.TeamAggregate.ValueObjects;

public sealed class Email : ValueObject
{
    private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    private Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Email cannot be empty.");
        }

        string normalized = value.Trim();
        if (!Regex.IsMatch(normalized, EmailPattern, RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250)))
        {
            throw new DomainException("Email format is invalid.");
        }

        Value = normalized;
    }

    public string Value { get; }

    public static Email Create(string value)
    {
        return new Email(value);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}