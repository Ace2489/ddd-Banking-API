using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User(Guid Id, string firstName, string lastName, string email, string phone, DateTime dateOfBirth) : Entity(Id)
{

    public string FirstName { get; private set; } = firstName;

    public string LastName { get; private set; } = lastName;

    public string Email { get; private set; } = email;

    public string Phone { get; private set; } = phone;

    public DateTimeOffset DateOfBirth { get; } = dateOfBirth;
}



