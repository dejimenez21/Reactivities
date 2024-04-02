using Application.IntegrationEvents.Users.Created;
using Application.Photos;
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
    private readonly IMediator _mediator;

    public AccountService(UserManager<ApplicationUser> userManager, TokenService tokenService, IMediator mediator)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mediator = mediator;
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

        await _mediator.Publish(new UserCreatedIntegrationEvent(user.Id, user.UserName, user.Email, user.Bio, user.DisplayName));

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

        var image = await _mediator.Send(new GetMain.Query { Username = user.UserName! });

        return new UserDto(user.UserName!, user.DisplayName, _tokenService.CreateToken(user), image);
    }

    private Result<UserDto> InvalidCredentialsError()
    {
        return new Error(401, "invalid.credentials", "Invalid email or password");
    }

    public async Task<UserDto?> GetUserByEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return null;

        var image = await _mediator.Send(new GetMain.Query { Username = user.UserName! });

        return new UserDto(user.UserName!, user.DisplayName, _tokenService.CreateToken(user), image);
    }
}
