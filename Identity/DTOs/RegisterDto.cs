namespace Identity.DTOs;

public record RegisterDto(string Username, string Password, string Email, string DisplayName)
{
}
