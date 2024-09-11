using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User(Guid Id, string firstName, string lastName, string email, string phone, DateTime dateOfBirth) : Entity(Id)
{

    [MaxLength(255)]
    public string FirstName { get; private set; } = firstName;

    [MaxLength(255)]
    public string LastName { get; private set; } = lastName;

    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; private set; } = email;

    [MaxLength(255)]
    public string Phone { get; private set; } = phone;

    public DateTimeOffset DateOfBirth { get; } = dateOfBirth;
}



