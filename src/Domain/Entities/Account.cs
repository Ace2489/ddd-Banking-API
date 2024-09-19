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
    public Account(Guid id, Guid userId, string accountNumber, AccountType accountType, Money initialBalance)
        : base(id)
    {
        UserId = userId;
        AccountNumber = accountNumber;
        Type = accountType;
        Balance = initialBalance;
        DateOpened = DateTimeOffset.UtcNow;
    }

    public Guid UserId { get; private set; }
    public string AccountNumber { get; private set; } = null!;
    public AccountType Type { get; private set; }
    public Money Balance { get; private set; } = null!;
    public DateTimeOffset DateOpened { get; private set; }

    public List<Transaction> Transactions => [.. _transactions];

    public Result<Account> Deposit(Money deposit, DateTimeOffset transactionTime)
    {
        Balance += deposit;
        _transactions.Add(new(Id, deposit, null, transactionTime, TransactionType.Deposit));

        return Result<Account>.Success(this);
    }

    public Result<Account> Withdraw(Money amount, DateTimeOffset transactionTime)
    {
        if (Balance < amount) return DomainErrors.Account.InsufficientFundsError;

        Balance = Money.Subtract(Balance, amount).Value!;
        Transaction transaction = new(Id, amount, null, transactionTime, TransactionType.Withdrawal);
        _transactions.Add(transaction);
        return this;
    }
}
