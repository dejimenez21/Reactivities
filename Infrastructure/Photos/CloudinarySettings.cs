#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Infrastructure.Photos;

public record CloudinarySettings
{
    internal const string SECTION_NAME = "Cloudinary";

    public string CloudName { get; set; }
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
}
