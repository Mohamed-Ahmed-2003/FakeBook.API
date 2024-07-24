using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.ChatRoomAggregate;
using MediatR;

namespace Fakebook.Application.CQRS.Chat.Queries
{
    public class GetChatMessages : IRequest<Response<List<ChatMessage>>>
    {
        public Guid RoomId { get; set; }
        public Guid UserProfileId { get; set; }

    }

    public class GetChatMessagesHandler : IRequestHandler<GetChatMessages, Response<List<ChatMessage>>>
    {
        public async Task<Response<List<ChatMessage>>> Handle(GetChatMessages request, CancellationToken cancellationToken)
        {
            var response = new Response<List<ChatMessage>>();

            // Implementation for getting chat messages

            // On success

            // On failure
            // response.AddError(StatusCodes.Status400BadRequest, "Error message");

            return response;
        }
    }

}
