using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Shared;

public interface IAuthenticationService
{
    Task<bool> ValidateCredentialsAsync(Email email, string password);
    Task<string> GenerateTokenAsync(Guid userId, Email email);
    Task<string> HashPassword(string password);
    Task<bool> VerifyPassword(string hashedPassword, string providedPassword);
}
//REgistering a user
/*
- Get credentials
- verify they do not exist
- hash password and create the user with hash
- store hash
*/
