using FakeBook.Domain.ValidationExceptions;
using FakeBook.Domain.Validators.MediaValidator;

namespace FakeBook.Domain.Aggregates.Shared
{
    public class Media
    {
        #region Properties
        public Guid Id { get; private set; }
        public string PublicId { get; private set; }
        public string Url { get; private set; }
        public MediaType MediaType { get; private set; }
        public DateTime UploadedAt { get; private set; }
        #endregion

        private Media() { }

        #region Factory Method
        public static Media CreateMedia(string publicId,string url, MediaType mediaType)
        {
            var media = new Media
            {
                Id = Guid.NewGuid(),
                Url = url,
                MediaType = mediaType,
                UploadedAt = DateTime.UtcNow,
                PublicId = publicId
            };

            ValidateMedia(media);

            return media;
        }
        #endregion

        #region Methods
        public void UpdateDetails(string url, MediaType mediaType)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new MediaNotValidException("Media URL cannot be empty.");
            }

            Url = url;
            MediaType = mediaType;
        }

        private static void ValidateMedia(Media media)
        {
            var validator = new MediaValidator();
            var validationResult = validator.Validate(media);
            if (!validationResult.IsValid)
            {
                var ex = new MediaNotValidException("Media is not valid.");
                foreach (var error in validationResult.Errors)
                {
                    ex.ValidationErrors.Add(error.ErrorMessage);
                }
                throw ex;
            }
        }
        #endregion
    }

    public enum MediaType
    {
        Image,
        Video,
        Audio
    }
}

