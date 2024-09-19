using Domain.Entities;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Withdrawal;

public record WithdrawalCommand : IRequest<Result<Account>>
{
    public Guid AccountId { get; }
    public Money Amount { get; }
    private WithdrawalCommand(Guid accountId, Money amount)
    {
        AccountId = accountId;
        Amount = amount;
    }

    public static Result<WithdrawalCommand> Create(Guid accountId, decimal amount)
    {
        Result<Money> moneyResult = Money.Create(amount);
        if (moneyResult.Value is null) return moneyResult.Error!;
        return new WithdrawalCommand(accountId, moneyResult.Value);
    }
}
