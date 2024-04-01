namespace Application.Photos;

public record PhotoUploadResult
{
    public string PublicId { get; set; }
    public string Url { get; set; }
}
