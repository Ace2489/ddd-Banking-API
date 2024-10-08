using System.Text;
using Application;
using Application.IRepository;
using Application.Shared;
using Infrastructure.Context;
using Infrastructure.Repository;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class Configure
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration["ConnectionStrings__PG_String"];
        services.AddNpgsql<AppDbContext>(connectionString);

        services
        .AddScoped<IAccountRepository, AccountRepository>()
        .AddScoped<IUserRepository, UserRepository>()
        .AddScoped<IUnitOfWork, UnitOfWork>();

        string? secret = configuration.GetSection("Jwt:Secret").Value;
        ArgumentException.ThrowIfNullOrWhiteSpace(secret);

        services
        .AddAuthentication(
            x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
        services
        .AddScoped<IAuthenticationService, AuthenticationService>()
        .AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();

        return services;
    }
}
