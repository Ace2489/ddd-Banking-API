using Domain.Shared;

namespace Application.Shared;

public static class ApplicationErrors
{
    public static Error AccountNotFoundError => new(nameof(AccountNotFoundError), "No account was found for the specified id");

    public static Error EmailAlreadyExistsError => new($"User.{nameof(EmailAlreadyExistsError)}", "A user already exists with the provided email address");
}
