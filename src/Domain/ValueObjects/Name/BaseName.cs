using Domain.Errors;
using Domain.Shared;

namespace Domain.ValueObjects;

public abstract record BaseName
{
    private static readonly int _maxLength = 100;

    public static int MaxLength => _maxLength;
    public string Name { get; }

    protected BaseName(string input) => Name = input;

    protected static Result<T> CreateName<T>(string firstName, Func<string, T> factory) where T : BaseName
    {
        if (string.IsNullOrWhiteSpace(firstName)) return DomainErrors.Name.EmptyInputError;
        if (firstName.Length > _maxLength) return DomainErrors.Name.MaxCharacterInputError;
        return factory(firstName);
    }
}
