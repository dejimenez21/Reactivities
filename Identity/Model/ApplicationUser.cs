using Microsoft.AspNetCore.Identity;

namespace Identity.Model;

public class ApplicationUser : IdentityUser
{
    public string? Bio { get; set; }
    public string DisplayName { get; set; }
}
