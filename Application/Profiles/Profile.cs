using Domain;

namespace Application.Profiles;

public record Profile
{
    public Profile(string username, string displayName, string? bio)
    {
        Username = username;
        DisplayName = displayName;
        Bio = bio;
    }
    public Profile()
    {
        
    }

    public string Username { get; set; }
    public string DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? Image { get; set; }
    public ICollection<Photo> Photos { get; set; }
}
