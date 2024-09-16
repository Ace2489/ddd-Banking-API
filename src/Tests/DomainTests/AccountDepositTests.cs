using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;

namespace Tests.DomainTests;

public class AccountDepositTests
{
    [Fact]
    public void Deposit_WithNegativeAmount_ReturnsError()
    {
        //arrange
        decimal negativeDeposit = -1000m;
        User testUser = new(Guid.NewGuid(), "first", "last", "email@email", "23456", new DateTime(2003, 12, 10));
        Account account = new(Guid.NewGuid(), testUser.Id, "22345", AccountType.Savings, new Money(1000m));

        //act
        Result<Account> result = account.Deposit(new Money(negativeDeposit), DateTimeOffset.UtcNow);

        //assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Error, DomainErrors.Account.InvalidDepositAmountError);
    }

    [Fact]
    public void Deposit_WithPositiveAmount_IncreasesAccountBalance()
    {
        Money positiveDeposit = new(1000m);
        Guid userId = Guid.NewGuid();
        Account account = new(Guid.NewGuid(), userId, "22345", AccountType.Savings, new Money(1000m));
        Money initialBalance = account.Balance;
        Money finalBalance = initialBalance + positiveDeposit;

        Result<Account> result = account.Deposit(positiveDeposit, DateTimeOffset.UtcNow);

        Assert.True(result.IsSuccess);
        Assert.Equal(finalBalance, result.Value!.Balance);
    }

    [Fact]
    public void DepositWhenSuccessful_AddsRelevantTransactionEntry()
    {
        Guid userId = Guid.NewGuid();
        string accountNumber = "123456789";
        Account account = new(Guid.NewGuid(), userId, accountNumber, AccountType.Savings, new Money(0));
        TransactionType transactionType = TransactionType.Deposit;
        DateTimeOffset transactionTime = DateTime.Now;
        var depositAmount = new Money(100m);

        Result<Account> result = account.Deposit(depositAmount, transactionTime);
        Account? returnedAccount = result.Value;
        Transaction transaction = account.Transactions.Single();

        Assert.True(result.IsSuccess);
        Assert.Equal(depositAmount, returnedAccount!.Balance);
        Assert.Single(returnedAccount.Transactions);

        Assert.Equal(depositAmount, transaction.Amount);
        Assert.Equal(account.Id, transaction.AccountId);
        Assert.Equal(transactionType, transaction.TransactionType);
        Assert.Equal(transactionTime, transaction.Timestamp);
    }
}