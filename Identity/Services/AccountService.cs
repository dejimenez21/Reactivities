using Application.IntegrationEvents.Users.Created;
using DDD.Foundation.Results;
using Identity.DTOs;
using Identity.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Services;

public class AccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TokenService _tokenService;
    private readonly IPublisher _publisher;

    public AccountService(UserManager<ApplicationUser> userManager, TokenService tokenService, IPublisher publisher)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _publisher = publisher;
    }

    public async Task<Result<UserDto>> RegisterNewUser(RegisterDto registerDto)
    {
        if (await _userManager.Users.AnyAsync(u => u.UserName == registerDto.Username))
        {
            var error = new Error(400, "register.user.validation", "One or more validation errors occurred");
            error.Reasons.Add(new(400, "username", "Username is already taken"));
            return error;
        }

        var user = new ApplicationUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Username
        };

        var identityResult = await _userManager.CreateAsync(user, registerDto.Password);

        if (!identityResult.Succeeded)
        {
            var error = new Error(400, "register.user.validation", "One or more validation errors occurred");
            error.Reasons.Add(new(400, "credentials", identityResult.Errors.FirstOrDefault()?.Description ?? "Problem occurred with credentials"));
            return error;
        }

        await _publisher.Publish(new UserCreatedIntegrationEvent(user.Id, user.UserName, user.Email, user.Bio, user.DisplayName));

        return new UserDto(user.UserName, user.DisplayName, _tokenService.CreateToken(user), null);
    }

    public async Task<Result<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return InvalidCredentialsError();
        }

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (result == false)
        {
            return InvalidCredentialsError();
        }

        return new UserDto(user.UserName!, user.DisplayName, _tokenService.CreateToken(user), null);
    }

    private Result<UserDto> InvalidCredentialsError()
    {
        return new Error(401, "invalid.credentials", "Invalid email or password");
    }

    public async Task<UserDto?> GetUserByEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        
        return user is not null ? new UserDto(user.UserName!, user.DisplayName, _tokenService.CreateToken(user), null) : null;
    }
}
