using Application.Features.Withdrawal;
using Domain.Errors;
using FluentAssertions;

namespace Tests.ApplicationTests.Withdrawal;

public class WithdrawalCommandTests
{
    [Fact]
    public void Create_WithValidInput_ShouldSucceed()
    {
        var accountId = Guid.NewGuid();
        decimal amount = 100m;

        var result = WithdrawalCommand.Create(accountId, amount);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.AccountId.Should().Be(accountId);
        result.Value!.Amount.Should().Be(Money.Create(amount).Value);
    }

    [Fact]
    public void Create_WithNegativeAmount_ShouldFail()
    {
        var accountId = Guid.NewGuid();
        decimal amount = -100m;

        var result = WithdrawalCommand.Create(accountId, amount);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(DomainErrors.Money.NegativeMoneyError);

    }
}
