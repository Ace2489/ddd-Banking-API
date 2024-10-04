using Bogus;
using Domain.Errors;
using Domain.ValueObjects.Name;
using FluentAssertions;

namespace Tests.DomainTests.ValueObjects;

public class FirstNameTests
{
    [Fact]
    public void FirstName_WithValidInput_ShouldReturnFirstName()
    {
        string validFirstName = "John";

        Result<FirstName> result = FirstName.Create(validFirstName);

        result.IsSuccess.Should().BeTrue();

        result.Value!.Name.Should().Be(validFirstName);
    }

    [Fact]
    public void FirstName_WithTooLongInput_ShouldReturnError()
    {
        Randomizer r = new();
        int characterLimit = BaseName.MaxLength;
        string invalidFirstName = new(r.Chars(count: characterLimit + 1));

        Result<FirstName> result = FirstName.Create(invalidFirstName);

        result.IsFailure.Should().BeTrue();

        result.Error!.Should().Be(DomainErrors.Name.MaxCharacterInputError);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void FirstName_WithEmptyStringInput_ShouldReturnError(string input)
    {

        Result<FirstName> result = FirstName.Create(input);

        result.IsFailure.Should().BeTrue();

        result.Error!.Should().Be(DomainErrors.Name.EmptyInputError);
    }

    [Fact]
    public void FirstName_WithSurroundingWhitespaces_ShouldTrimWhitespace()
    {
        string whitespacedName = " First ";

        Result<FirstName> result = FirstName.Create(whitespacedName);

        result.IsSuccess.Should().BeTrue();

        result.Value!.Name.Should().Be(whitespacedName.Trim());
    }
}
