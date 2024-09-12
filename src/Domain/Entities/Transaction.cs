
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Transaction(Guid Id, Guid accountId, Money amount, string? description, DateTimeOffset transactionTimestamp, TransactionType transactionType, Guid? referenceId = null) : Entity(Id)
{
    public Guid AccountId { get; } = accountId;
    public Money Amount { get; } = amount;
    public string? Description { get; } = description;
    public TransactionType TransactionType { get; } = transactionType;
    public DateTimeOffset Timestamp { get; } = transactionTimestamp;
    public Guid? ReferenceId { get; } = referenceId;

}
