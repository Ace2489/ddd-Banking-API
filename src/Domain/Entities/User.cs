using Domain.Shared;
using Domain.ValueObjects;
using Domain.ValueObjects.Name;

namespace Domain.Entities;

public class User : Entity
{
    private readonly List<Account> _accounts = [];
    //For EF Core
    private User() { }
    private User(Guid Id, FirstName firstName, LastName lastName, Email email, string phone, DateTimeOffset dateOfBirth, string passwordHash) : base(Id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        DateOfBirth = dateOfBirth;
        PasswordHash = passwordHash;
    }


    public FirstName FirstName { get; private set; } = null!;

    public LastName LastName { get; private set; } = null!;

    public Email Email { get; private set; } = null!;

    public string Phone { get; private set; } = null!;

    public string PasswordHash { get; private set; } = null!;

    public List<Account> Accounts => [.. _accounts];
    public DateTimeOffset DateOfBirth { get; }

    public static Result<User> Create(Guid Id, FirstName firstName, LastName lastname, Email email, string phone, DateTimeOffset dateOfBirth, string passwordHash)
    {
        User user = new(Id, firstName, lastname, email, phone, dateOfBirth, passwordHash);
        Account userAccount = Account.Create(Guid.NewGuid(), user.Id).Value!;
        user._accounts.Add(userAccount);
        return user;
    }
}



