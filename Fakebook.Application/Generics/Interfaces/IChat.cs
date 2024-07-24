using FakeBook.Domain.Aggregates.ChatRoomAggregate;

namespace Fakebook.Application.Generics.Interfaces
{
    public interface IChatNotifier
    {
        Task NotifyChatRoomCreated(ChatRoom chatRoom);
        Task NotifyMessageSent( ChatMessage chatMessage);

    }
}
