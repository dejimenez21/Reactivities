#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Application.Photos;

public record PhotoUploadResult
{
    public string PublicId { get; set; }
    public string Url { get; set; }
}
