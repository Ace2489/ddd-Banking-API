using Domain.Entities;
using Domain.ValueObjects.Name;
using FluentAssertions;

namespace Tests.DomainTests;

public class UserTest
{
    [Fact]
    public void Create_WithValidCredentials_ShouldCreateANewUser()
    {
        Email email = Email.Create("email@email.com").Value!;
        FirstName firstName = FirstName.Create("firstName").Value!;
        LastName lastName = LastName.Create("lastName").Value!;
        string phone = "phone";
        string hashedPassword = "passwordHash";
        var dateOfBirth = DateTimeOffset.UtcNow;

        var userResult = User.Create(Guid.NewGuid(), firstName, lastName, email, phone, dateOfBirth, hashedPassword);

        userResult.IsSuccess.Should().BeTrue();
        userResult.Value.Should().NotBeNull();

        User user = userResult.Value!;

        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Email.Should().Be(email);
        user.DateOfBirth.Should().Be(dateOfBirth);
        user.Phone.Should().Be(phone);
    }

    [Fact]
    public void Create_WithValidCredentials_ShouldCreateAnAccountForTheNewUser()
    {
        Email email = Email.Create("email@email.com").Value!;
        FirstName firstName = FirstName.Create("firstName").Value!;
        LastName lastName = LastName.Create("lastName").Value!;
        string phone = "phone";
        string hashedPassword = "passwordHash";
        var dateOfBirth = DateTimeOffset.UtcNow;

        var userResult = User.Create(Guid.NewGuid(), firstName, lastName, email, phone, dateOfBirth, hashedPassword);

        User user = userResult.Value!;

        user.Accounts.Should().HaveCount(1);
        user.Accounts.Single().OwnerId.Should().Be(user.Id);
    }


}
