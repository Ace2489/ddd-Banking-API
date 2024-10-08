using Domain.Errors;
using Domain.Shared;

namespace Domain.ValueObjects;

public sealed record Email
{
    private static readonly int maxLength = 255;

    public static int MaxLength => maxLength;

    public string Mail { get; } = null!;

    private Email(string email) => Mail = email;

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return DomainErrors.Email.EmptyInputError;
        if (email.Length > maxLength) return DomainErrors.Email.MaxCharacterInputError;
        if (!email.Contains('@')) return DomainErrors.Email.InvalidEmailError;
        return new Email(email);
    }
}
