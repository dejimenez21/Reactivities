﻿#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Application.Activities;

public record AttendeeDto
{
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? Image { get; set; }
}
