using Identity.DTOs;
using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var result = await _accountService.Login(loginDto);

        return result.IsSuccess ? result.Value : new ObjectResult(result.Error!.Message) { StatusCode = result.Error.StatusCode };
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        var result = await _accountService.RegisterNewUser(registerDto); 
        
        if(result.IsFailed)
        {
            result.Error!.Reasons.ForEach(reason => ModelState.AddModelError("password", reason.Message));
            return ValidationProblem();
        }

        return result.IsSuccess ? result.Value : new ObjectResult(result.Error!.Message) { StatusCode = result.Error.StatusCode };
    }

    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _accountService.GetUserByEmail(User.FindFirstValue(ClaimTypes.Email)!);

        return user!;
    }
}
