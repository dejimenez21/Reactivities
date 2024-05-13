#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Microsoft.AspNetCore.Identity;

namespace Identity.Model;

public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; }
}
