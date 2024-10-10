using Application.Features.History;
using Domain.Errors;
using FluentAssertions;

namespace Tests.ApplicationTests.History;

public class HistoryCommandTests
{
    [Fact]
    public void Create_WithValidDates_ShouldReturnSuccessResult()
    {
        // Arrange
        var start = DateTimeOffset.UtcNow;
        var end = start.AddDays(1);
        var accountId = Guid.NewGuid();

        // Act
        var result = HistoryCommand.Create(start, end, accountId);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Period.Start.Should().Be(start);
        result.Value!.Period.End.Should().Be(end);
    }

    [Fact]
    public void Create_WithEndDateBeforeStartDate_ShouldReturnFailureResult()
    {
        // Arrange
        var start = DateTimeOffset.UtcNow;
        var end = start.AddDays(-1);
        var accountId = Guid.NewGuid();

        // Act
        var result = HistoryCommand.Create(start, end, accountId);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.DateTimePeriod.NegativePeriodError);
    }
}
