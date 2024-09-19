using Domain.Entities;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Deposit;

public record DepositCommand : IRequest<Result<Account>>
{
    public Guid AccountId { get; init; }
    public Money Amount { get; init; }
    private DepositCommand(Guid accountId, Money money)
    {
        AccountId = accountId;
        Amount = money;
    }

    public static Result<DepositCommand> Create(Guid accountId, decimal money)
    {
        Result<Money> moneyResult = Money.Create(money);

        return moneyResult.IsSuccess ? new DepositCommand(accountId, moneyResult.Value!) : moneyResult.Error!;
    }

    public static DepositCommand Create(Guid accountId, Money money)
    {
        return new DepositCommand(accountId, money);
    }
}
