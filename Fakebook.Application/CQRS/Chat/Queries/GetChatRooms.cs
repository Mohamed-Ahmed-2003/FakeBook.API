using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.ChatRoomAggregate;
using MediatR;

namespace Fakebook.Application.CQRS.Chat.Queries
{
    public class GetChatRoomsQuery : IRequest<Response<List<ChatRoom>>>
    {
        public Guid UserProfileId { get; set; }
    }
    public class GetChatRoomsQueryHandler : IRequestHandler<GetChatRoomsQuery, Response<List<ChatRoom>>>
    {
        public async Task<Response<List<ChatRoom>>> Handle(GetChatRoomsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<ChatRoom>>();

            // Implementation for getting chat rooms

            // On success

            // On failure
            // response.AddError(StatusCodes.Status400BadRequest, "Error message");

            return response;
        }
    }

}
