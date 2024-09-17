using Domain.Errors;
using Domain.Shared;

namespace Domain.ValueObjects;

public record Money
{
    public decimal Value { get; init; }
    private Money(decimal value) => Value = value;
    public static Result<Money> Create(decimal amount)
    {
        if (amount < 0) return DomainErrors.Money.NegativeMoneyError;
        return new Money(amount);
    }

    public static Money operator +(Money first, Money second) => first with { Value = second.Value + first.Value };
}
