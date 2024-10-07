using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.Shared.Models;

public record UserResponse(string FirstName, string LastName, string Email, string Phone, DateTimeOffset DateOfBirth, [property: JsonIgnore] string PasswordHash, IEnumerable<AccountResponse> Accounts)
{
    public static explicit operator UserResponse(User user)
    {
        IEnumerable<AccountResponse> accounts = user.Accounts.Select(a => (AccountResponse)a);
        return new(user.FirstName.Name, user.LastName.Name, user.Email.Mail, user.Phone, user.DateOfBirth, user.PasswordHash, accounts);
    }
}

