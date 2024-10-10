using Domain.Entities;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Deposit;

public record DepositCommand : IRequest<Result<Account>>
{
    public Guid AccountId { get; init; }
    public Money Amount { get; init; }
    public Guid UserId { get; }

    private DepositCommand(Guid accountId, Money money, Guid userId)
    {
        AccountId = accountId;
        Amount = money;
        UserId = userId;
    }

    public static Result<DepositCommand> Create(Guid accountId, decimal money, Guid userId)
    {
        Result<Money> moneyResult = Money.Create(money);

        return moneyResult.IsSuccess ? new DepositCommand(accountId, moneyResult.Value!, userId) : moneyResult.Error!;
    }
}
