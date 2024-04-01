namespace Infrastructure.Photos;

public record CloudinarySettings
{
    internal const string SECTION_NAME = "Cloudinary";

    public string CloudName { get; set; }
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
}
