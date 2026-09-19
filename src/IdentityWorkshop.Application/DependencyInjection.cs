using IdentityWorkshop.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityWorkshop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddWorkshopApplication(this IServiceCollection services)
    {
        services.AddScoped<UserService>();
        return services;
    }
}

