using Domain.Shared;
using Domain.ValueObjects;
using MediatR;

namespace Application.Auth.Login;

public record LoginCommand : IRequest<Result<LoginResponse>>
{
    public Email Email { get; }

    public string Password { get; }

    private LoginCommand(Email email, string password)
    {
        Email = email;
        Password = password;
    }

    public static Result<LoginCommand> Create(string email, string password)
    {
        Result<Email> mail = Email.Create(email);
        if (mail.Value is null) return mail.Error!;
        return new LoginCommand(mail.Value, password);
    }

}
