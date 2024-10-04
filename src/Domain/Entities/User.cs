using Domain.ValueObjects.Name;

namespace Domain.Entities;

public class User : Entity
{
    private readonly List<Account> _accounts = [];
    //For EF Core
    private User() { }
    public User(Guid Id, FirstName firstName, LastName lastName, string email, string phone, DateTimeOffset dateOfBirth, string password) : base(Id)
    {
        FirstName = firstName;
        LastName = lastName;
        // Email = email;
        Phone = phone;
        DateOfBirth = dateOfBirth;
        Password = password;
    }


    public FirstName FirstName { get; private set; } = null!;

    public LastName LastName { get; private set; } = null!;

    // public Email Email { get; private set; } = null!;

    public string Phone { get; private set; } = null!;

    public string Password { get; private set; } = null!;

    public List<Account> Accounts => [.. _accounts];
    public DateTimeOffset DateOfBirth { get; }
}



