using Domain.Shared;

namespace Application.Shared;

public static class ApplicationErrors
{
    public static Error AccountNotFoundError => new(nameof(AccountNotFoundError), "No account was found for the specified id");

    public static Error EmailAlreadyExistsError => new($"User.{nameof(EmailAlreadyExistsError)}", "A user already exists with the provided email address");

    public static Error InvalidUserDetails => new($"User.{nameof(InvalidUserDetails)}", "No user exists for the provided details");
}
