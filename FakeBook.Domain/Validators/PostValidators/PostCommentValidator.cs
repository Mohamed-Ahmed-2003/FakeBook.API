using FakeBook.Domain.Aggregates.PostAggregate;
using FluentValidation;


namespace FakeBook.Domain.Validators.PostValidators
{
    public class PostCommentValidator : AbstractValidator<PostComment>
    {
        public PostCommentValidator()
        {
            RuleFor(pc => pc.CommentText)
                .NotNull().WithMessage("Comment text should not be null")
                .NotEmpty().WithMessage("Comment text should not be empty")
                .MaximumLength(1000)
                .MinimumLength(1);
        }
    }
}
