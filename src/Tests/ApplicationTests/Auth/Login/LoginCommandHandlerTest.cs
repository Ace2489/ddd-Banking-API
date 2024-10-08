using Application;
using Application.Auth.Login;
using Application.IRepository;
using Application.Shared;
using Application.Shared.Models;
using Domain.Entities;
using Domain.ValueObjects.Name;
using FluentAssertions;
using NSubstitute;

namespace Tests.ApplicationTests.Auth.Login;

public class LoginCommandHandlerTest
{
    [Fact]
    public async void Handle_WithValidInput_ShouldSucceed()
    {
        Email email = Email.Create("email@email.com").Value!;
        FirstName firstName = FirstName.Create("firstName").Value!;
        LastName lastName = LastName.Create("lastName").Value!;
        DateTimeOffset dateOfBirth = DateTimeOffset.UtcNow;
        string phone = "phone";
        string password = "password";
        string passwordHash = "passwordHash lol";
        User? testUser = User.Create(Guid.NewGuid(), firstName, lastName, email, phone, dateOfBirth, passwordHash).Value;

        //substitutes
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.FindByEmail(email).Returns(testUser);
        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.SaveChangesAsync().Returns(1);
        var authService = Substitute.For<IAuthenticationService>();
        authService.VerifyPassword(passwordHash, password).Returns(true);

        var command = LoginCommand.Create(email.Mail, password).Value!;
        var handler = new LoginCommandHandler(userRepository, authService);
        Result<LoginResponse> loginResponse = await handler.Handle(command, default);


        loginResponse.IsSuccess.Should().BeTrue();
        UserResponse loggedInUser = loginResponse.Value!.User;
        loggedInUser.Should().BeEquivalentTo((UserResponse)testUser!);

        await authService.Received(1).GenerateTokenAsync(testUser!.Id, testUser.Email);

    }

    [Fact]
    public async void Handle_WithNonExistentUser_ShouldReturnError()
    {
        Email email = Email.Create("email@email.com").Value!;
        string password = "password";
        //substitutes
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.FindByEmail(email).Returns((User)null!);
        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.SaveChangesAsync().Returns(1);
        var authService = Substitute.For<IAuthenticationService>();

        var command = LoginCommand.Create(email.Mail, password).Value!;
        var handler = new LoginCommandHandler(userRepository, authService);
        Result<LoginResponse> loginResponse = await handler.Handle(command, default);

        loginResponse.IsFailure.Should().BeTrue();
        loginResponse.Error!.Should().Be(ApplicationErrors.AccountNotFoundError);
    }
}
