using Fakebook.Application.Options;
using FakeBook.Domain.Aggregates.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace FakeBook.API.Filters
{
    public class FileUploadValidationAttribute : TypeFilterAttribute
    {
        public FileUploadValidationAttribute() : base(typeof(FileValidationFilter))
        {
        }
    }

    public class FileValidationFilter (IOptions<MediaSettings> settings) : ActionFilterAttribute
    {
        private readonly MediaSettings _mediaSettings = settings.Value;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var files = context.HttpContext.Request.Form.Files;
            foreach (var file in files)
            {
                var mimeType = file.ContentType;
                var size = file.Length;

                if (_mediaSettings.SupportedImageMimeTypes.Contains(mimeType))
                {
                    file.Headers["FileFormat"] = MediaType.Image.ToString();
                    if (size > _mediaSettings.MaxImageSize)
                    {
                        context.Result = new BadRequestObjectResult($"Image file size exceeds the limit of {_mediaSettings.MaxImageSize} bytes.");
                        return;
                    }
                }
                else if (_mediaSettings.SupportedVideoMimeTypes.Contains(mimeType))
                {
                    file.Headers["FileFormat"] = MediaType.Video.ToString();
                    if (size > _mediaSettings.MaxVideoSize)
                    {
                        context.Result = new BadRequestObjectResult($"Video file size exceeds the limit of {_mediaSettings.MaxVideoSize} bytes.");
                        return;
                    }
                }
                else if (_mediaSettings.SupportedAudioMimeTypes.Contains(mimeType))
                {
                    file.Headers["FileFormat"] = MediaType.Audio.ToString();
                    if (size > _mediaSettings.MaxAudioSize)
                    {
                        context.Result = new BadRequestObjectResult($"Audio file size exceeds the limit of {_mediaSettings.MaxAudioSize} bytes.");
                        return;
                    }
                }
                else
                {
                    context.Result = new BadRequestObjectResult($"File type {mimeType} is not supported.");
                    return;
                }
            }
        }
    }
}
