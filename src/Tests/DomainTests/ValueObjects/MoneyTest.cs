using Domain.Shared;
using Domain.ValueObjects;
using FluentAssertions;

namespace Tests.DomainTests.ValueObjects;

public class MoneyTest
{
    [Fact]
    public void Money_ShouldReturnError_ForNegativeAmount()
    {
        decimal negativeAmount = -1000m;

        Result<Money> result = Money.Create(negativeAmount);

        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
    }
}
