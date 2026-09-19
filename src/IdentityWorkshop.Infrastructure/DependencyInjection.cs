using IdentityWorkshop.Application.Users;
using IdentityWorkshop.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityWorkshop.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddWorkshopInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<Persistence.WorkshopDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}

