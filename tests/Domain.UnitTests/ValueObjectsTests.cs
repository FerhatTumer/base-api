using FluentAssertions;
using TaskManagement.Domain.Aggregates.TeamAggregate.ValueObjects;
using TaskManagement.Domain.Common;
using TaskManagement.Domain.Enums;
using Xunit;

namespace TaskManagement.Domain.UnitTests;

public sealed class ValueObjectsTests
{
    [Fact]
    public void CreateEmail_WithInvalidFormat_ThrowsDomainException()
    {
        Action act = () => Email.Create("invalid-email");

        act.Should().Throw<DomainException>()
            .WithMessage("*invalid*");
    }

    [Fact]
    public void CreateMoney_WithNegativeAmount_ThrowsDomainException()
    {
        Action act = () => Money.Create(-1m, Currency.USD);

        act.Should().Throw<DomainException>()
            .WithMessage("*cannot be negative*");
    }

    [Fact]
    public void CreateMoney_WithSameAmountAndCurrency_AreEqual()
    {
        Money left = Money.Create(120.50m, Currency.EUR);
        Money right = Money.Create(120.50m, Currency.EUR);

        left.Should().Be(right);
    }
}
