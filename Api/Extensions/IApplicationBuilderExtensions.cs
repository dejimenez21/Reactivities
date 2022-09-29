using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Api.Extensions;

public static class IApplicationBuilderExtensions
{
    /// <summary>
    /// Creates or updates the database to the last migration before runing the application.
    /// </summary>
    /// <param name="app"></param>
    public static void InitializeDatabase(this IApplicationBuilder app)
    {
        using var scopedServices = app.ApplicationServices.CreateScope();

        try 
        {
            var context = scopedServices.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
            Seed.SeedData(context);
        }
        catch(Exception ex)
        {
            var logger =  scopedServices.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occured during migration");
        }
    }
}
