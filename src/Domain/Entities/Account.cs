using Domain.Enums;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities;


public class Account(Guid Id, Guid userId, string accountNumber, AccountType accountType) : Entity(Id)
{
    public Guid UserId { get; } = userId;
    public string AccountNumber { get; } = accountNumber;
    public AccountType Type { get; private set; } = accountType;
    public Money Balance { get; private set; } = new Money(0);
    public DateTimeOffset DateOpened { get; private set; } = DateTimeOffset.UtcNow;

    public Result<Account> Deposit(Money deposit)
    {
        if (deposit.Amount < 0) return DomainErrors.Account.InvalidDepositAmountError;
        return new Error("code", "str");
    }
}
