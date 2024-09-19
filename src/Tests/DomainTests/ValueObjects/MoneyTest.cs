using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;
using FluentAssertions;

namespace Tests.DomainTests.ValueObjects;

public class MoneyTest
{
    [Fact]
    public void Money_ShouldReturnMoney_ForPositiveAmount()
    {
        decimal positiveAmount = 1000m;

        Result<Money> result = Money.Create(positiveAmount);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Value.Should().Be(positiveAmount);
    }
    [Fact]
    public void Money_ShouldReturnError_ForNegativeAmount()
    {
        decimal negativeAmount = -1000m;

        Result<Money> result = Money.Create(negativeAmount);

        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void Subtract_ShouldReturnMoney_ForGreaterMinuend()
    {
        Money minuend = Money.Create(1000).Value!;
        Money subtrahend = Money.Create(500).Value!;

        var result = Money.Subtract(minuend, subtrahend);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Value.Should().Be(500);
    }

    [Fact]
    public void Subtract_ShouldReturnError_ForGreaterSubtrahend()
    {
        Money minuend = Money.Create(100).Value!;
        Money subtrahend = Money.Create(500).Value!;

        var result = Money.Subtract(minuend, subtrahend);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(DomainErrors.Money.NegativeMoneyError);
    }
}
