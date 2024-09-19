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

    public static bool operator <(Money first, Money second) => first.Value < second.Value;

    public static bool operator >(Money first, Money second) => first.Value > second.Value;
    public static Result<Money> Subtract(Money minuend, Money subtrahend)
    {
        if (minuend < subtrahend) return DomainErrors.Money.NegativeMoneyError;
        return minuend with { Value = minuend.Value - subtrahend.Value };
    }


}
