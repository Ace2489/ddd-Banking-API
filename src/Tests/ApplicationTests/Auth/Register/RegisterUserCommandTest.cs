using Application.Auth.Register;
using Application.Shared;
using Domain.Errors;
using FluentAssertions;

namespace Tests.ApplicationTests.Auth.Register;

public class RegisterUserCommandTest
{
    [Fact]
    public void Create_WithValidInput_ShouldSucceed()
    {
        string firstName = "first";
        string lastName = "last";
        string email = "email@email.com";
        string phone = "phone";
        string password = "pass";
        DateTimeOffset dateOfBirth = DateTimeOffset.UtcNow;

        var result = RegisterUserCommand.Create(firstName, lastName, email, password, phone, dateOfBirth);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        RegisterUserCommand command = result.Value!;

        command.FirstName.Name.Should().Be(firstName);
        command.LastName.Name.Should().Be(lastName);
        command.Email.Mail.Should().Be(email);
        command.Phone.Should().Be(phone);
        command.Password.Should().Be(password);
        command.DateOfBirth.Should().Be(dateOfBirth);
    }

    [Fact]
    public void Create_WithInvalidData_ReturnsAllErrors()
    {
        // Arrange
        var firstName = "";
        var lastName = "";
        var email = "invalid-email";
        string password = "valid-pass";
        var phone = "valid-phone";
        var dateOfBirth = DateTimeOffset.UtcNow;

        // Act
        var result = RegisterUserCommand.Create(
            firstName, lastName, email, password, phone, dateOfBirth);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ValidationError>();
        var validationError = (ValidationError)result.Error!;
        validationError.Should().NotBeNull();
        validationError.Errors.Should().Contain(DomainErrors.Name.EmptyInputError);
        validationError.Errors.Should().Contain(DomainErrors.Email.InvalidEmailError);
    }
}
