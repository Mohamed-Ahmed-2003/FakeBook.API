using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
namespace Fakebook.Application.Generics.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
