using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Configure
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}
