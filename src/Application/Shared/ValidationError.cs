using Domain.Shared;

namespace Application.Shared;

public record ValidationError : Error
{
    public IReadOnlyCollection<Error> Errors { get; }

    private ValidationError(string message, IEnumerable<Error> errors) : base("Validation.Error", message)
    {
        Errors = errors.ToList().AsReadOnly();
    }

    public static ValidationError UserValidationError(IEnumerable<Error> errors) => new("An error occurred when validating user profiles", errors);
}
