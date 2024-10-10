using Domain.Errors;
using FluentAssertions;

namespace Tests.DomainTests.ValueObjects;

public class DateTimePeriodTests
{
    private static class Data
    {
        public static DateTimeOffset Start => new(new DateTime(2024, 10, 09));
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
        Result<DateTimePeriod> period = DateTimePeriod.Create(Data.End, Data.Start);

        period.IsFailure.Should().BeTrue();
        period.Error.Should().Be(DomainErrors.DateTimePeriod.NegativePeriodError);
    }
}
