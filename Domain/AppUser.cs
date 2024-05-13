#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Domain;

public class AppUser
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string? Bio { get; set; }
    public ICollection<ActivityAttendee> Activities { get; set; }
    public ICollection<Photo> Photos { get; set; }
}
