#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Domain;

public class Photo
{
    public string Id { get; set; }
    public string Url { get; set; }
    public bool IsMain { get; set; }
}
