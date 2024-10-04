using Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace Tests.Integration;

internal class BankAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
            string? connectionString = GetConnectionString();
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

            IServiceProvider provider = services.BuildServiceProvider();
            IServiceScope scope = provider.CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Database.EnsureDeleted();
        });
    }

    private static string? GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder().AddUserSecrets<BankAppFactory>().Build();
        return configuration["ConnectionStrings__PG_String"];
    }
}
