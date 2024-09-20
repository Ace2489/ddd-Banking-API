﻿using Application;
using Domain.Repository;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Configure
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration["ConnectionStrings__PG_String"];
        services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
