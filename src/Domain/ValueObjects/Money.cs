namespace Domain.ValueObjects;

public record Money(decimal Amount)
{
    public static Money operator +(Money first, Money second) => first with {Amount = second.Amount};
}
