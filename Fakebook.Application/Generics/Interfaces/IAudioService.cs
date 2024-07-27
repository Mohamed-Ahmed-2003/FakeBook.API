using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Fakebook.Application.Generics.Interfaces
{
    public interface IAudioService
    {
        Task<RawUploadResult> AddAudioAsync(IFormFile file);
        Task<DeletionResult> DeleteAudioAsync(string publicId);
    }

}
