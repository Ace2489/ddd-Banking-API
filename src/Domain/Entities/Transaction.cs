
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Transaction : Entity
{
    //For EF Core
    // private Transaction() { }

    public Transaction() { }
    public Transaction(Guid accountId, Money amount, string? description, DateTimeOffset transactionTimestamp, TransactionType transactionType, Guid? referenceId = null)
    {
        AccountId = accountId;
        Amount = amount;
        Description = description;
        Timestamp = transactionTimestamp;
        TransactionType = transactionType;
        ReferenceId = referenceId;
    }
    public Guid AccountId { get; private set; }
    public Money Amount { get; private set; } = null!;
    public string? Description { get; private set; }
    public TransactionType TransactionType { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }
    public Guid? ReferenceId { get; private set; }

}
