using Domain.Enums;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Account : Entity
{
    private readonly List<Transaction> _transactions = [];

    //For EF Core
    private Account() { }
    private Account(Guid id, Guid userId, string accountNumber, AccountType accountType, Money initialBalance)
        : base(id)
    {
        OwnerId = userId;
        AccountNumber = accountNumber;
        Type = accountType;
        Balance = initialBalance;
        DateOpened = DateTimeOffset.UtcNow;
    }

    public Guid OwnerId { get; private set; }
    public string AccountNumber { get; private set; } = null!;
    public AccountType Type { get; private set; }
    public Money Balance { get; private set; } = null!;
    public DateTimeOffset DateOpened { get; private set; }

    public List<Transaction> Transactions => [.. _transactions];

    public Result<Account> Deposit(Money deposit, DateTimeOffset transactionTime)
    {
        Balance += deposit;
        _transactions.Add(new(Id, deposit, null, transactionTime, TransactionType.Debit));

        return Result<Account>.Success(this);
    }

    public Result<Account> Withdraw(Money amount, DateTimeOffset transactionTime)
    {
        if (Balance < amount) return DomainErrors.Account.InsufficientFundsError;

        Balance = Money.Subtract(Balance, amount).Value!;
        Transaction transaction = new(Id, amount, null, transactionTime, TransactionType.Credit);
        _transactions.Add(transaction);
        return this;
    }

    internal static Result<Account> Create(Guid accountId, Guid ownerId, AccountType accountType = AccountType.Savings)
    {
        Money initialBalance = Money.Create(0).Value!;
        return new Account(accountId, ownerId, GetRandomString(10), accountType, initialBalance);
    }

    internal static Result<Account> CreateWithInitialBalance(Guid accountId, Guid ownerId, Money initialBalance, AccountType accountType = AccountType.Savings)
    {
        Account account = Create(accountId, ownerId, accountType).Value!;
        account.Deposit(initialBalance, DateTimeOffset.UtcNow);
        return account;
    }

    private static string GetRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        Random random = new();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
