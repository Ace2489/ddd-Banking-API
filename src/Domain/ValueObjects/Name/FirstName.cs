using Domain.Shared;

namespace Domain.ValueObjects.Name;

public sealed record FirstName : BaseName
{
    private FirstName(string input) : base(input) { }

    public static Result<FirstName> Create(string input) => CreateName(input, nameInput => new FirstName(nameInput.Trim()));

}

