using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Web.Extensions;

public static class MigrationExtensions
{
    public static IServiceProvider Migrate(this IServiceProvider services)
    {
        IServiceScope serviceScope = services.CreateScope();
        var db = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
        return services;
    }
}
