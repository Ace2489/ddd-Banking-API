namespace Domain.Entities;

public class User : Entity
{
    //For EF Core
    private User() { }
    public User(Guid Id, string firstName, string lastName, string email, string phone, DateTime dateOfBirth) : base(Id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        DateOfBirth = dateOfBirth;
    }

    public string FirstName { get; private set; } = null!;

    public string LastName { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string Phone { get; private set; } = null!;

    public DateTimeOffset DateOfBirth { get; }
}



