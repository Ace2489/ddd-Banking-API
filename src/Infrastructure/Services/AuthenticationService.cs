using Application.Shared;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class AuthenticationService(IPasswordHasher<object> passwordHasher) : IAuthenticationService
{
    private readonly IPasswordHasher<object> passwordHasher = passwordHasher; //asp.net identity doesn't actually use the user object when hashing

    public Task<string> GenerateTokenAsync(Guid userId, Email email)
    {
        return Task.FromResult("a dummy token");
    }

    public Task<string> HashPassword(string password)
    {
        return Task.FromResult(passwordHasher.HashPassword(new { }, password));
    }

    public Task<bool> ValidateCredentialsAsync(Email email, string password)
    {
        throw new NotImplementedException();
    }

    public Task<bool> VerifyPassword(string hashedPassword, string providedPassword)
    {
        PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(new { }, hashedPassword, providedPassword);
        return Task.FromResult(result == PasswordVerificationResult.Success);
    }
}
