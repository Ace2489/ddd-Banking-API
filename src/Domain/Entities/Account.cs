using Domain.Enums;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities;


public class Account(Guid Id, Guid userId, string accountNumber, AccountType accountType) : Entity(Id)
{
    private readonly List<Transaction> _transactions = [];
    public Guid UserId { get; } = userId;
    public string AccountNumber { get; } = accountNumber;
    public AccountType Type { get; private set; } = accountType;
    public Money Balance { get; private set; } = new Money(0);
    public DateTimeOffset DateOpened { get; } = DateTimeOffset.UtcNow;
    public List<Transaction> Transactions => [.. _transactions];
    public Result<Account> Deposit(Money deposit, DateTimeOffset transactionTime)
    {
        if (deposit.Amount < 0) return DomainErrors.Account.InvalidDepositAmountError;
        Balance += deposit;
        _transactions.Add(new(Guid.NewGuid(), Id, deposit, null, transactionTime, TransactionType.Deposit));
        return this;
    }
}
