using Domain.Entities;
using Domain.Enums;

namespace Application.Shared.Models;

public record TransactionResponse(Guid Id, Guid AccountId, decimal Amount, string? Description, DateTimeOffset Timestamp, TransactionType TransactionType, Guid? ReferenceId)
{
    public static explicit operator TransactionResponse(Transaction transaction) =>
    new(
        transaction.Id,
        transaction.AccountId,
        transaction.Amount.Value!,
        transaction.Description,
        transaction.Timestamp,
        transaction.TransactionType,
        transaction.ReferenceId
        );
}
