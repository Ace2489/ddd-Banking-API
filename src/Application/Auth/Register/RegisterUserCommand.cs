using Application.Shared;
using Domain.Shared;
using Domain.ValueObjects;
using Domain.ValueObjects.Name;
using MediatR;

namespace Application.Auth.Register;

public sealed record RegisterUserCommand : IRequest<Result<RegistrationResponse>>
{

    public FirstName FirstName { get; }
    public LastName LastName { get; }
    public Email Email { get; }
    public string Password { get; }
    public string Phone { get; }
    public DateTimeOffset DateOfBirth { get; }
    private RegisterUserCommand(FirstName firstName, LastName lastName, Email email, string password, string phone, DateTimeOffset dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Phone = phone;
        DateOfBirth = dateOfBirth;
    }

    public static Result<RegisterUserCommand> Create(string firstName, string lastName, string email, string password, string phone, DateTimeOffset dateOfBirth)
    {
        var errors = new List<Error>();
        var first = FirstName.Create(firstName);
        if (first.Error is not null) errors.Add(first.Error);

        var last = LastName.Create(lastName);
        if (last.Error is not null) errors.Add(last.Error);

        var mail = Email.Create(email);
        if (mail.Error is not null) errors.Add(mail.Error);

        if (errors.Count != 0) return ValidationError.UserValidationError(errors);
        return new RegisterUserCommand(first.Value!, last.Value!, mail.Value!, password, phone, dateOfBirth);
    }
}
