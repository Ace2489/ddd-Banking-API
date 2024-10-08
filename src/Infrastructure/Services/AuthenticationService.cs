using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Shared;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class AuthenticationService(IPasswordHasher<object> passwordHasher, IConfiguration configuration, ILogger<AuthenticationService> logger) : IAuthenticationService
{
    private readonly IPasswordHasher<object> passwordHasher = passwordHasher; //asp.net identity doesn't actually use the user object when hashing
    private readonly IConfiguration configuration = configuration;

    public Task<string> GenerateTokenAsync(Guid userId, Email email)
    {
        List<Claim> claims =
        [
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Name, email.Mail)
        ];

        string? keyString = configuration.GetSection("Jwt:Secret").Value;
        ArgumentException.ThrowIfNullOrWhiteSpace(keyString);
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(keyString));

        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        string tokenString = tokenHandler.WriteToken(token);

        return Task.FromResult(tokenString);
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
