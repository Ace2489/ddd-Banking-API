using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using FluentAssertions;

namespace Tests.DomainTests.AccountTests;

public class AccountWithdrawalTests
{
    [Fact]
    public void Withdraw_WithValidAmount_DecreasesTheBalance()
    {
        var userId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var withdrawalAmount = Money.Create(10m).Value!;
        var initialBalance = withdrawalAmount + Money.Create(100).Value!;

        Account account = Account.CreateWithInitialBalance(accountId, userId, initialBalance, AccountType.Checking).Value!;
        var result = account.Withdraw(withdrawalAmount, DateTimeOffset.UtcNow);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().Be(account);
        account.Balance.Should().Be(Money.Subtract(initialBalance, withdrawalAmount).Value);
    }
    [Fact]
    public void Withdraw_WithValidAmount_AddsTransactionEntry()
    {
        var userId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var withdrawalAmount = Money.Create(10m).Value!;
        var initialBalance = withdrawalAmount + Money.Create(10).Value!;
        DateTimeOffset transactionTime = DateTimeOffset.UtcNow;

        Account account = Account.CreateWithInitialBalance(accountId, userId, initialBalance, AccountType.Savings).Value!;
        var result = account.Withdraw(withdrawalAmount, transactionTime);

        result.IsSuccess.Should().BeTrue();
        Transaction transaction = account.Transactions[1]; //Initial deposit adds one transaction entry
        transaction.Amount.Should().Be(withdrawalAmount);
        transaction.TransactionType.Should().Be(TransactionType.Credit);
        transaction.Timestamp.Should().Be(transactionTime);
    }

    [Fact]
    public void Withdraw_WithInsufficientFunds_ShouldFail()
    {
        // Arrange
        var initialBalance = Money.Create(10).Value!;
        var account = Account.CreateWithInitialBalance(Guid.NewGuid(), Guid.NewGuid(), initialBalance, AccountType.Checking).Value!;
        var withdrawalAmount = Money.Create(1000).Value!;

        // Act
        var result = account.Withdraw(withdrawalAmount, DateTimeOffset.UtcNow);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Account.InsufficientFundsError);
        account.Balance.Should().Be(initialBalance);
        account.Transactions.Should().HaveCount(1);//Initial deposit adds one transaction entry
    }
}
