using Domain.Errors;
using Domain.Shared;

namespace Domain.ValueObjects;

public record DateTimePeriod
{
    public DateTimeOffset Start { get; }

    public DateTimeOffset End { get; }

    private DateTimePeriod(DateTimeOffset start, DateTimeOffset end)
    {
        Start = start;
        End = end;
    }

    public static Result<DateTimePeriod> Create(DateTimeOffset start, DateTimeOffset end)
    {
        if (start > end) return DomainErrors.DateTimePeriod.NegativePeriodError;
        return new DateTimePeriod(start, end);
    }

    public bool Contains(DateTimeOffset dateTime)
    {
        return Start <= dateTime && End >= dateTime;
    }
}
