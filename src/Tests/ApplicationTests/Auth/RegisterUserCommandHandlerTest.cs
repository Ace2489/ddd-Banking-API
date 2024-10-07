using Application;
using Application.Authentication;
using Application.IRepository;
using Application.Shared;
using Application.Shared.Models;
using Domain.Entities;
using Domain.ValueObjects.Name;
using FluentAssertions;
using NSubstitute;

namespace Tests.ApplicationTests.Auth;

public class RegisterUserCommandHandlerTest
{
    [Fact]
    public async void Handle_WithValidUserCredentials_ShouldCreateAUser()
    {
        Email email = Email.Create("email@email.com").Value!;
        FirstName firstName = FirstName.Create("firstName").Value!;
        LastName lastName = LastName.Create("lastName").Value!;
        DateTimeOffset dateOfBirth = DateTimeOffset.UtcNow;
        string phone = "phone";
        string password = "password";

        //substitutes
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.FindByEmail(email).Returns((User)null!);
        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.SaveChangesAsync().Returns(1);
        var authService = Substitute.For<IAuthenticationService>();

        RegisterUserCommand command = RegisterUserCommand.Create(firstName.Name, lastName.Name, email.Mail, password, phone, dateOfBirth).Value!;

        var handler = new RegisterUserCommandHandler(userRepository, authService, unitOfWork);
        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        UserResponse user = result.Value!.User;

        user.FirstName.Should().Be(firstName.Name);
        user.LastName.Should().Be(lastName.Name);
        user.Email.Should().Be(email.Mail);
        user.Phone.Should().Be(phone);
        user.DateOfBirth.Should().Be(dateOfBirth);

        await unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async void Handle_WithValidUserCredentials_ShouldHashThePassword()
    {

        Email email = Email.Create("email@email.com").Value!;
        FirstName firstName = FirstName.Create("firstName").Value!;
        LastName lastName = LastName.Create("lastName").Value!;
        DateTimeOffset dateOfBirth = DateTimeOffset.UtcNow;
        string phone = "phone";
        string password = "unhashed password";
        string hashedPassword = "A hashed password lol";

        //subs
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.FindByEmail(email).Returns((User)null!);
        var authService = Substitute.For<IAuthenticationService>();
        authService.HashPassword(password).Returns(hashedPassword);
        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.SaveChangesAsync().Returns(1);

        RegisterUserCommand command = RegisterUserCommand.Create(firstName.Name, lastName.Name, email.Mail, password, phone, dateOfBirth).Value!;


        var handler = new RegisterUserCommandHandler(userRepository, authService, unitOfWork);
        var result = await handler.Handle(command, default);

        result.Value!.User.PasswordHash.Should().NotBe(password);

    }

    [Fact]
    public async void Handle_WithExistingEmail_ShouldReturnError()
    {

        Email email = Email.Create("email@email.com").Value!;
        FirstName firstName = FirstName.Create("firstName").Value!;
        LastName lastName = LastName.Create("lastName").Value!;
        DateTimeOffset dateOfBirth = DateTimeOffset.UtcNow;
        string phone = "phone";
        string password = "unhashed password";

        User user = User.Create(Guid.NewGuid(), firstName, lastName, email, phone, dateOfBirth, password).Value!; //only here for the return

        var userRepository = Substitute.For<IUserRepository>();
        userRepository.FindByEmail(email).Returns(user);
        var authService = Substitute.For<IAuthenticationService>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.SaveChangesAsync().Returns(1);


        RegisterUserCommand command = RegisterUserCommand.Create(firstName.Name, lastName.Name, email.Mail, password, phone, dateOfBirth).Value!;


        var handler = new RegisterUserCommandHandler(userRepository, authService, unitOfWork);
        var result = await handler.Handle(command, default);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(ApplicationErrors.EmailAlreadyExistsError);
    }
    // [Fact]
    // public async void Handle_WithValidUserCredentials_ShouldCreateAToken()
    // {
    //     Email email = Email.Create("email@email.com").Value!;
    //     FirstName firstName = FirstName.Create("firstName").Value!;
    //     LastName lastName = LastName.Create("lastName").Value!;
    //     DateTimeOffset dateOfBirth = DateTimeOffset.UtcNow;
    //     string phone = "phone";
    //     string password = "unhashed password";
    //     string hashedPassword = "A hashed password lol";

    //     var userRepository = Substitute.For<IUserRepository>();
    //     userRepository.FindByEmail(email).Returns((User)null!);
    //     var authService = Substitute.For<IAuthenticationService>();
    //     authService.HashPassword(password).Returns(hashedPassword);
    //     RegisterUserCommand command = RegisterUserCommand.Create(firstName.Name, lastName.Name, email.Mail, password, phone, dateOfBirth).Value!;

    //     var handler = new RegisterUserCommandHandler(userRepository, authService);
    //     var result = await handler.Handle(command, default);
    // }
}
