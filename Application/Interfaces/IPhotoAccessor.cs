using Application.Core;
using Application.Photos;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IPhotoAccessor
{
    Task<Result<PhotoUploadResult>> AddPhoto(IFormFile file);
    /// <summary>
    /// Try to delete photo from cloudinary given a public Id
    /// </summary>
    /// <param name="publicId">The public Id of the photo</param>
    /// <returns>A boolean value indicating if the photo was deleted successfully</returns>
    Task<bool> DeletePhoto(string publicId);
}
