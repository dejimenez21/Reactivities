﻿using Application.Profiles;

namespace Application.Activities;

public record ActivityDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTimeOffset Date { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string City { get; set; }
    public string Venue { get; set; }
    public string HostUsername { get; set; }
    public bool IsCanceled { get; set; }
    public IEnumerable<Profile> Attendees { get; set; }
}
