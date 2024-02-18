using DDD.Foundation.Results;
using Identity.DTOs;
using Identity.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Services;

public class AccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TokenService _tokenService;

    public AccountService(UserManager<ApplicationUser> userManager, TokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<Result<UserDto>> RegisterNewUser(RegisterDto registerDto)
    {
        if (await _userManager.Users.AnyAsync(u => u.UserName == registerDto.Username))
        {
            return new Error(400, "username.already.taken", "Username is already taken");
        }

        var user = new ApplicationUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Username
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return new Error(400, "register.user.failed", result.Errors.FirstOrDefault()?.Description ?? "Problem registering user");
        }

        return new UserDto(user.UserName, user.DisplayName, _tokenService.CreateToken(user), null);

    }

    public async Task<Result<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return new Error(401, "invalid.credentials", "Incorrect email or password");
        }

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (result == false)
        {
            return new Error(401, "invalid.credentials", "Incorrect email or password");
        }

        return new UserDto(user.UserName!, user.DisplayName, _tokenService.CreateToken(user), null);
    }

    public async Task<UserDto?> GetUserByEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        
        return user is not null ? new UserDto(user.UserName!, user.DisplayName, _tokenService.CreateToken(user), null) : null;
    }
}
