using Domain.Errors;
using FluentAssertions;

namespace Tests.DomainTests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Create_WithValidEmail_ShouldReturnSuccess()
    {
        string validEmail = "test@email.com";

        var result = Email.Create(validEmail);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Mail.Should().Be(validEmail.Trim());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithEmptyEmail_ShouldReturnError(string emptyEmail)
    {
        var result = Email.Create(emptyEmail);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Email.EmptyInputError);
    }

    [Fact]
    public void Create_WithExceededMaxCharacterInput_ShouldReturnError()
    {
        int maxCharacters = Email.MaxLength;
        string BeyondMaxString = new('a', maxCharacters + 1);

        var result = Email.Create(BeyondMaxString);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Email.MaxCharacterInputError);
    }

    [Fact]
    public void Create_WithInvalidEmail_ShouldReturnError()
    {
        string invalidEmail = "email"; // no @

        var result = Email.Create(invalidEmail);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Email.InvalidEmailError);
    }
}
