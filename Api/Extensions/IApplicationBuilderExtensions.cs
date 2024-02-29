using Identity;
using Identity.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Api.Extensions;

public static class IApplicationBuilderExtensions
{
    /// <summary>
    /// Creates or updates the database to the last migration before runing the application.
    /// </summary>
    /// <param name="app"></param>
    public static async Task InitializeDatabase(this IApplicationBuilder app)
    {
        using var scopedServices = app.ApplicationServices.CreateScope();

        try 
        {
            var context = scopedServices.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
            var identityContext = scopedServices.ServiceProvider.GetRequiredService<IdentityContext>();
            identityContext.Database.Migrate();
            var userManager = scopedServices.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var publisher = scopedServices.ServiceProvider.GetRequiredService<IPublisher>();
            await Seed.SeedData(context, userManager, publisher);
        }
        catch(Exception ex)
        {
            var logger =  scopedServices.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occured during migration");
        }
    }
}
