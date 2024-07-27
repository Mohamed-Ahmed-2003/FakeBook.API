using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Fakebook.Application.Generics.Interfaces
{
    public interface IVideoService
    {
        Task<VideoUploadResult> AddVideoAsync(IFormFile file);
        Task<DeletionResult> DeleteVideoAsync(string publicId);
    }

}
