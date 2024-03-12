using Application.Interfaces;
using System.Security.Claims;

namespace Api.Services;

public class UserContext : IUserContext
{
    public string UserName => _user.FindFirstValue(ClaimTypes.Name)!;
    public string Id => _user.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private readonly ClaimsPrincipal _user;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _user = httpContextAccessor.HttpContext!.User;
    }
}
