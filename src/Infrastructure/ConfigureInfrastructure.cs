using Application;
using Application.IRepository;
using Application.Shared;
using Infrastructure.Context;
using Infrastructure.Repository;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        services
        .AddScoped<IAuthenticationService, AuthenticationService>()
        .AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();

        return services;
    }
}
