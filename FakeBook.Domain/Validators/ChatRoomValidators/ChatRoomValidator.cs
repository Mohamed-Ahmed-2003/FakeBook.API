using FakeBook.Domain.Aggregates.ChatRoomAggregate;
using FluentValidation;

namespace FakeBook.Domain.Validators.ChatValidators
{
    public class ChatRoomValidator : AbstractValidator<ChatRoom>
    {
        public ChatRoomValidator()
        {
            RuleFor(chatRoom => chatRoom.Name).NotEmpty().WithMessage("Chat room name cannot be empty.");
        }
    }
}
