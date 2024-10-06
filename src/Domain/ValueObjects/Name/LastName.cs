using Domain.Shared;

namespace Domain.ValueObjects.Name;

public sealed record LastName : BaseName
{
    private LastName(string input) : base(input) { }

    public static Result<LastName> Create(string input) => CreateName(input, nameInput => new LastName(nameInput.Trim()));
}

