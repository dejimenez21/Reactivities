using Domain;

namespace Application.Profiles;

public record Profile
{
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? Image { get; set; }
    public ICollection<Photo> Photos { get; set; }
}
