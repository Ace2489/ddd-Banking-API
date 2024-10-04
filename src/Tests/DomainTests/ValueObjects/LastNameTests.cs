using Bogus;
using Domain.Errors;
using Domain.ValueObjects.Name;
using FluentAssertions;

namespace Tests.DomainTests.ValueObjects;

public class LastNameTest
{
    [Fact]
    public void LastName_WithValidInput_ShouldReturnLastName()
    {
        string validLastName = "John";

        Result<LastName> result = LastName.Create(validLastName);

        result.IsSuccess.Should().BeTrue();

        result.Value!.Name.Should().Be(validLastName);
    }

    [Fact]
    public void LastName_WithTooLongInput_ShouldReturnError()
    {
        Randomizer r = new();
        int characterLimit = 50;
        string invalidLastName = new(r.Chars(count: characterLimit + 1));

        Result<LastName> result = LastName.Create(invalidLastName);

        result.IsFailure.Should().BeTrue();

        result.Error!.Should().Be(DomainErrors.Name.BeyondMaxLimitError);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void LastName_WithEmptyStringInput_ShouldReturnError(string input)
    {

        Result<LastName> result = LastName.Create(input);

        result.IsFailure.Should().BeTrue();

        result.Error!.Should().Be(DomainErrors.Name.EmptyInputError);
    }
    [Fact]
    public void LastName_WithSurroundingWhitespaces_ShouldTrimWhitespace()
    {
        string whitespacedName = " Last ";

        Result<LastName> result = LastName.Create(whitespacedName);

        result.IsSuccess.Should().BeTrue();

        result.Value!.Name.Should().Be(whitespacedName.Trim());
    }
}
