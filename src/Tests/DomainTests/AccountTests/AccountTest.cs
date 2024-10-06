using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Tests.DomainTests.AccountTests;

public class AccountTest
{
    [Fact]
    public void Create_WithValidDetails_ShouldCreateAnEmptyAccountForAUser()
    {
        Guid accountId = Guid.NewGuid();
        Guid ownerId = Guid.NewGuid();
        AccountType accountType = AccountType.Checking;

        var accountResult = Account.Create(accountId, ownerId, accountType);

        accountResult.Value!.Should().NotBeNull();

        Account account = accountResult.Value!;
        account.Id.Should().Be(accountId);
        account.OwnerId.Should().Be(ownerId);
        account.Type.Should().Be(accountType);
    }

    [Fact]
    public void CreateWithInitialBalance_WithValidDetails_ShouldCreateAnAccountWithAnInitialBalance()
    {
        Guid accountId = Guid.NewGuid();
        Guid ownerId = Guid.NewGuid();
        AccountType accountType = AccountType.Checking;
        Money initialBalance = Money.Create(1000).Value!;

        var accountResult = Account.CreateWithInitialBalance(accountId, ownerId, initialBalance, accountType);

        accountResult.Value.Should().NotBeNull();

        Account account = accountResult.Value!;
        account.Id.Should().Be(accountId);
        account.OwnerId.Should().Be(ownerId);
        account.Type.Should().Be(accountType);
        account.Balance.Should().Be(initialBalance);
    }
}
