using FakeBook.Domain.Aggregates.Shared;
using FluentValidation;


namespace FakeBook.Domain.Validators.MediaValidator
{
    public class MediaValidator : AbstractValidator<Media>
    {
        public MediaValidator()
        {
            RuleFor(media => media.Url).NotEmpty().WithMessage("URL cannot be empty.");
        }
    }
}
