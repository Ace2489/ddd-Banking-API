using Domain.Entities;
using Domain.Enums;

namespace Application.Shared.Models;

public record AccountResponse(Guid AccountId, string AccountNumber, Guid OwnerId, AccountType AccountType, decimal Balance)
{
    public static explicit operator AccountResponse(Account account) => new(account.Id, account.AccountNumber, account.OwnerId, account.Type, account.Balance.Value);
}

