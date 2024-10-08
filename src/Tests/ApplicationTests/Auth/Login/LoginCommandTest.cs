using Application.Auth.Login;
using Domain.Errors;
using FluentAssertions;

namespace Tests.ApplicationTests.Auth.Login;

public class LoginCommandTest
{
    [Fact]
    public void Create_WithValidInput_ShouldSucceed()
    {
        string email = "email@";
        string password = "password";

        var result = LoginCommand.Create(email, password);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Email.Mail.Should().Be(email);
        result.Value!.Password.Should().Be(password);
    }

    [Fact]
    public void Create_WithInvalidData_ReturnsError()
    {
        string invalidEmail = "email";
        string password = "password";

        var result = LoginCommand.Create(invalidEmail, password);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Email.InvalidEmailError);
    }
}
