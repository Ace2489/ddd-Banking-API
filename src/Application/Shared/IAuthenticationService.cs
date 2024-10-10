using Domain.ValueObjects;

namespace Application.Shared;

public interface IAuthenticationService
{
    Task<string> GenerateTokenAsync(Guid userId, Email email);
    Task<string> HashPassword(string password);
    Task<bool> VerifyPassword(string hashedPassword, string providedPassword);
}

