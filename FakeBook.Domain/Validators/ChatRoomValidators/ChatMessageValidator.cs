using FluentValidation;
using FakeBook.Domain.Aggregates.ChatRoomAggregate;

namespace FakeBook.Domain.Validators.ChatValidators
{
    public class ChatMessageValidator : AbstractValidator<ChatMessage>
    {
        public ChatMessageValidator()
        {
            RuleFor(message => message.ChatRoomId)
                .NotEmpty()
                .WithMessage("Chat room ID cannot be empty.");

            RuleFor(message => message.UserProfileId)
                .NotEmpty()
                .WithMessage("User profile ID cannot be empty.");

            RuleFor(message => message.Content)
                .NotEmpty()
                .WithMessage("Message content cannot be empty.");

     
        }
    }
}
