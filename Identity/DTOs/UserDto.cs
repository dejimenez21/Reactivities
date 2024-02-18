namespace Identity.DTOs;

public record UserDto(string Username, string DisplayName, string Token, string Image)
{
}
