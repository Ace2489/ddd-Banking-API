using Infrastructure.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace Tests.Integration.Setup;

internal class BankAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            //Switch the db to a test db
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
            string? connectionString = GetConnectionString();
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
            services.AddNpgsql<AppDbContext>(connectionString);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "TestScheme";
                options.DefaultChallengeScheme = "TestScheme";
                options.DefaultScheme = "TestScheme";
            }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });


            //Make sure we start with a fresh test db
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

    private static Guid _guid = Guid.NewGuid();
    public static Guid UserId => _guid; 
}
