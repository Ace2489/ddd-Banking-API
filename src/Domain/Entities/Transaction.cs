
namespace Domain.Entities;

public class Transaction(Guid Id, Guid accountId, decimal amount, string? description, DateTimeOffset transactionTimestamp, Guid? referenceId = null) : Entity(Id)
{
    public Guid AccountId { get; } = accountId;
    public decimal Amount { get; } = amount;
    public string? Description { get; } = description;
    public DateTimeOffset Timestamp { get; } = transactionTimestamp;
    public Guid? ReferenceId { get; } = referenceId;

}
