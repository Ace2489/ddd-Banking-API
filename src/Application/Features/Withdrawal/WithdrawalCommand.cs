using Domain.Entities;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Withdrawal;

public record WithdrawalCommand : IRequest<Result<Account>>
{
    public Guid AccountId { get; }
    public Money Amount { get; }
    public Guid UserId { get; }

    private WithdrawalCommand(Guid accountId, Money amount, Guid userId)
    {
        AccountId = accountId;
        Amount = amount;
        UserId = userId;
    }

    public static Result<WithdrawalCommand> Create(Guid accountId, decimal amount, Guid userId)
    {
        Result<Money> moneyResult = Money.Create(amount);
        if (moneyResult.Value is null) return moneyResult.Error!;
        return new WithdrawalCommand(accountId, moneyResult.Value, userId);
    }
}
