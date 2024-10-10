using Domain.Entities;

namespace Application.Shared.Models;

public record AccountResponse(Guid AccountId, string AccountNumber, Guid OwnerId, string AccountType, decimal Balance)
{
    public static explicit operator AccountResponse(Account account) => new(account.Id, account.AccountNumber, account.OwnerId, account.Type.ToString(), account.Balance.Value);
}

