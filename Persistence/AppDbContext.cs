using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Activity> Activities { get; set; }

        

        #pragma warning disable CS8618
        public AppDbContext(DbContextOptions options) : base(options) {}

    }
}