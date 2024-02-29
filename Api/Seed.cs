using Application.IntegrationEvents.Users.Created;
using Domain;
using Identity.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Api;
public class Seed
{
    public static async Task SeedData(AppDbContext context, UserManager<ApplicationUser> userManager, IPublisher publisher)
    {
        if(!userManager.Users.Any())
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser{DisplayName = "Bob", UserName = "bob", Email = "bob@test.com"},
                new ApplicationUser{DisplayName = "Tom", UserName = "tom", Email = "Tom@test.com"},
                new ApplicationUser{DisplayName = "Jane", UserName = "jane", Email = "jane@test.com"},
            };

            foreach (var user in users)
            {
                var result = await userManager.CreateAsync(user, "Passw0rd");
                if (result.Succeeded)
                {
                    await publisher.Publish(new UserCreatedIntegrationEvent(user.Id, user.UserName, user.Email, user.Bio, user.DisplayName));
                }
            }
        }

        if(context.Activities.Any()) return;

        var activities = new List<Activity>
            {
                new Activity
                {
                    Title = "Past Activity 1",
                    Date = DateTimeOffset.UtcNow.AddMonths(-2),
                    Description = "Activity 2 months ago",
                    Category = "drinks",
                    City = "London",
                    Venue = "Pub",
                },
                new Activity
                {
                    Title = "Past Activity 2",
                    Date = DateTimeOffset.UtcNow.AddMonths(-1),
                    Description = "Activity 1 month ago",
                    Category = "culture",
                    City = "Paris",
                    Venue = "Louvre",
                },
                new Activity
                {
                    Title = "Future Activity 1",
                    Date = DateTimeOffset.UtcNow.AddMonths(1),
                    Description = "Activity 1 month in future",
                    Category = "culture",
                    City = "London",
                    Venue = "Natural History Museum",
                },
                new Activity
                {
                    Title = "Future Activity 2",
                    Date = DateTimeOffset.UtcNow.AddMonths(2),
                    Description = "Activity 2 months in future",
                    Category = "music",
                    City = "London",
                    Venue = "O2 Arena",
                },
                new Activity
                {
                    Title = "Future Activity 3",
                    Date = DateTimeOffset.UtcNow.AddMonths(3),
                    Description = "Activity 3 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Another pub",
                },
                new Activity
                {
                    Title = "Future Activity 4",
                    Date = DateTimeOffset.UtcNow.AddMonths(4),
                    Description = "Activity 4 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Yet another pub",
                },
                new Activity
                {
                    Title = "Future Activity 5",
                    Date = DateTimeOffset.UtcNow.AddMonths(5),
                    Description = "Activity 5 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Just another pub",
                },
                new Activity
                {
                    Title = "Future Activity 6",
                    Date = DateTimeOffset.UtcNow.AddMonths(6),
                    Description = "Activity 6 months in future",
                    Category = "music",
                    City = "London",
                    Venue = "Roundhouse Camden",
                },
                new Activity
                {
                    Title = "Future Activity 7",
                    Date = DateTimeOffset.UtcNow.AddMonths(7),
                    Description = "Activity 2 months ago",
                    Category = "travel",
                    City = "London",
                    Venue = "Somewhere on the Thames",
                },
                new Activity
                {
                    Title = "Future Activity 8",
                    Date = DateTimeOffset.UtcNow.AddMonths(8),
                    Description = "Activity 8 months in future",
                    Category = "film",
                    City = "London",
                    Venue = "Cinema",
                }
            };

        context.Activities.AddRange(activities);
        await context.SaveChangesAsync();
    }
}
