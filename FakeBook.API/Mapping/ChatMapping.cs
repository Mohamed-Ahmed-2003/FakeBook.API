using AutoMapper;
using FakeBook.API.Contracts.Chat.Responses;
using FakeBook.Domain.Aggregates.ChatRoomAggregate;

namespace FakeBook.API.Mapping
{
    public class ChatMapping :Profile
    {
        ChatMapping()
        {
            CreateMap<ChatMessage, AbstractChatMessage>()
                .ForMember(m=>m.ChatMessageId,src =>src.MapFrom(m=>m.Id));

            CreateMap<ChatRoom, AbstractChatRoom>()
                .ForMember(r => r.ChatRoomId, src => src.MapFrom(r => r.Id))
                .ForMember(r => r.Messages, src => src.MapFrom(r => r.Messages));

        }
    }
}
