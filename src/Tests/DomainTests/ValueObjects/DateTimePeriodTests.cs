using Domain.Errors;
using FluentAssertions;

namespace Tests.DomainTests.ValueObjects;

public class DateTimePeriodTests
{
    private static class Data
    {
        public static DateTimeOffset Start => new(new DateTime(2024, 10, 09));
        public static DateTimeOffset ContainedPoint => new(new DateTime(2024, 10, 09, 20, 00, 00));
        public static DateTimeOffset End => new(new DateTime(2024, 10, 10));
    }
    [Fact]
    public void Create_WithValidParameters_ShouldReturnDateTimePeriod()
    {
        Result<DateTimePeriod> period = DateTimePeriod.Create(Data.Start, Data.End);

        period.IsSuccess.Should().BeTrue();
        period.Value!.Start.Should().Be(Data.Start);
        period.Value!.End.Should().Be(Data.End);
    }

    [Fact]
    public void Create_WithInvalidPeriod_ShouldReturnError()
    {
        Result<DateTimePeriod> periodResult = DateTimePeriod.Create(Data.End, Data.Start);

        periodResult.IsFailure.Should().BeTrue();
        periodResult.Error.Should().Be(DomainErrors.DateTimePeriod.NegativePeriodError);
    }

    [Fact]
    public void Contains_WithBoundedDateTime_ShouldReturnTrue()
    {
        DateTimePeriod period = DateTimePeriod.Create(Data.Start, Data.End).Value!;

        bool contained = period.Contains(Data.ContainedPoint);

        contained.Should().BeTrue();
    }

    [Fact]
    public void Contains_WithUnboundedDateTime_ShouldReturnFalse()
    {
        DateTimePeriod period = DateTimePeriod.Create(Data.Start, Data.End).Value!;

        bool unContained = period.Contains(Data.End.AddDays(1));

        unContained.Should().BeFalse();
    }
}
