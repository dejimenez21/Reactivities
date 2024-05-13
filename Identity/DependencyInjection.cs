using Identity.Model;
using Identity.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureIdentity(this IServiceCollection services, IConfiguration configuration, string EnvironmentName)
    {
        services = ConfigureDbContexts(services, configuration, EnvironmentName);

        services.AddIdentityCore<ApplicationUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
            opt.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<IdentityContext>();

        services.AddMediatR(typeof(IdentityContext).Assembly);

        services.AddScoped<TokenService>();
        services.AddScoped<AccountService>();

        return services;
    }

    private static IServiceCollection ConfigureDbContexts(IServiceCollection services, IConfiguration config, string EnvironmentName)
    {
        switch (EnvironmentName)
        {
            case "Development":
                services.AddDbContext<IdentityContext>(opt =>
                {
                    opt.UseNpgsql(config.GetConnectionString("DefaultConnection"));
                });
                break;
            case "Local":
                services.AddDbContext<IdentityContext>(opt =>
                {
                    opt.UseSqlite(config.GetConnectionString("DefaultConnection")!);
                });
                break;
            default:
                break;
        }
        return services;
    }
}
