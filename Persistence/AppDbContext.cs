using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Activity> Activities { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<UserFollowing> UserFollowings { get; set; }


#pragma warning disable CS8618
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ActivityAttendee>()
            .HasKey(aa => new { aa.AppUserId, aa.ActivityId });

        modelBuilder.Entity<ActivityAttendee>()
            .HasOne(aa => aa.AppUser)
            .WithMany(u => u.Activities)
            .HasForeignKey(aa => aa.AppUserId);

        modelBuilder.Entity<ActivityAttendee>()
            .HasOne(aa => aa.Activity)
            .WithMany(a => a.Attendees)
            .HasForeignKey(aa => aa.ActivityId);

        modelBuilder.Entity<Activity>()
            .HasMany(a => a.Comments)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserFollowing>(b =>
        {
            b.HasKey(x => new { x.ObserverId, x.TargetId });

            b.HasOne(x => x.Target)
            .WithMany(a => a.Followers)
            .HasForeignKey(x => x.TargetId)
            .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Observer)
            .WithMany(a => a.Followings)
            .HasForeignKey(x => x.ObserverId)
            .OnDelete(DeleteBehavior.Cascade);
        });
    }
}