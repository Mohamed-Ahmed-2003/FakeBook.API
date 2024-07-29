using FakeBook.Domain.Aggregates.PostAggregate;
using FluentValidation;
namespace FakeBook.Domain.Validators.PostValidators
{
    public class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
        {
            RuleFor(p => p.Text)
                .NotNull().WithMessage("Post text content can't be null")
                .NotEmpty().WithMessage("Post text content can't be empty");

        }
    }
}
