namespace Fakebook.Application.Options
{
    public class MediaSettings
    {
        public string[] SupportedImageMimeTypes { get; set; }
        public string[] SupportedVideoMimeTypes { get; set; }
        public string[] SupportedAudioMimeTypes { get; set; }
        public long MaxImageSize { get; set; } // in bytes
        public long MaxVideoSize { get; set; } // in bytes
        public long MaxAudioSize { get; set; } // in bytes
    }
}
