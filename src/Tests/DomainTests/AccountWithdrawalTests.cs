using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using FluentAssertions;

namespace Tests.DomainTests;

public class AccountWithdrawalTests
{
    [Fact]
    public void Withdraw_WithValidAmount_DecreasesTheBalance()
    {
        var userId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var accountNumber = "12345";
        var initialBalance = Money.Create(100m).Value!;
        var withdrawalAmount = Money.Create(10m).Value!;

        Account account = new(accountId, userId, accountNumber, AccountType.Checking, initialBalance);

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
        var accountNumber = "12345";
        var initialBalance = Money.Create(100m).Value!;
        var withdrawalAmount = Money.Create(10m).Value!;
        DateTimeOffset transactionTime = DateTimeOffset.UtcNow;

        Account account = new(accountId, userId, accountNumber, AccountType.Checking, initialBalance);

        var result = account.Withdraw(withdrawalAmount, transactionTime);
        
        result.IsSuccess.Should().BeTrue();
        Transaction transaction = account.Transactions.Single();
        transaction.Amount.Should().Be(withdrawalAmount);
        transaction.TransactionType.Should().Be(TransactionType.Withdrawal);
        transaction.Timestamp.Should().Be(transactionTime);
    }

    [Fact]
    public void Withdraw_WithInsufficientFunds_ShouldFail()
    {
        // Arrange
        var account = new Account(Guid.NewGuid(), Guid.NewGuid(), "123456", AccountType.Checking, Money.Create(500).Value!);
        var withdrawalAmount = Money.Create(1000).Value!;

        // Act
        var result = account.Withdraw(withdrawalAmount, DateTimeOffset.UtcNow);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Account.InsufficientFundsError);
        account.Balance.Value.Should().Be(500);
        account.Transactions.Should().BeEmpty();
    }
}
