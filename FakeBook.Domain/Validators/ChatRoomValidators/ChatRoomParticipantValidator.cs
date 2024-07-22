using FluentValidation;
using FakeBook.Domain.Aggregates.ChatRoomAggregate;

namespace FakeBook.Domain.Validators.ChatValidators
{
    public class ChatRoomParticipantValidator : AbstractValidator<ChatRoomParticipant>
    {
        public ChatRoomParticipantValidator()
        {
            RuleFor(participant => participant.ChatRoomId)
                .NotEmpty()
                .WithMessage("Chat room ID cannot be empty.");

            RuleFor(participant => participant.UserProfileId)
                .NotEmpty()
                .WithMessage("User profile ID cannot be empty.");
        }
    }
}
