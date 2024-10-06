using Domain.Entities;
using Domain.Enums;

namespace Tests.DomainTests.AccountTests;

public class AccountDepositTests
{

    [Fact]
    public void Deposit_IncreasesAccountBalance()
    {
        Money deposit = Money.Create(1000m).Value!;
        Guid userId = Guid.NewGuid();
        AccountType accountType = AccountType.Savings;

        Account account = Account.Create(Guid.NewGuid(), userId, accountType).Value!;
        Money initialBalance = account.Balance;
        Money finalBalance = initialBalance + deposit;

        Result<Account> result = account.Deposit(deposit, DateTimeOffset.UtcNow);

        Assert.True(result.IsSuccess);
        Assert.Equal(finalBalance, result.Value!.Balance);
    }

    [Fact]
    public void Deposit_WhenSuccessful_AddsRelevantTransactionEntry()
    {
        Guid userId = Guid.NewGuid();
        Guid accountId = Guid.NewGuid();
        AccountType accountType = AccountType.Savings;
        Account account = Account.Create(accountId, userId, accountType).Value!;

        TransactionType transactionType = TransactionType.Deposit;
        DateTimeOffset transactionTime = DateTime.Now;
        var depositAmount = Money.Create(1000m).Value!;

        Result<Account> result = account.Deposit(depositAmount, transactionTime);
        Account returnedAccount = result.Value!;
        Transaction transaction = account.Transactions.Single();

        Assert.True(result.IsSuccess);
        Assert.Single(returnedAccount.Transactions);

        Assert.Equal(depositAmount, transaction.Amount);
        Assert.Equal(account.Id, transaction.AccountId);
        Assert.Equal(transactionType, transaction.TransactionType);
        Assert.Equal(transactionTime, transaction.Timestamp);
    }
}