using Domain.Shared;

namespace Application.Shared;

public static class ApplicationErrors
{
    public static Error AccountNotFoundError => new(nameof(AccountNotFoundError), "No account was found for the specified id");

}
