using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.ChatRoomAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.CQRS.Chat.Queries
{
    public class GetChatMessages : IRequest<Response<List<ChatMessage>>>
    {
        public Guid RoomId { get; set; }
        public Guid UserProfileId { get; set; }

    }

    public class GetChatMessagesHandler(DataContext context) : IRequestHandler<GetChatMessages, Response<List<ChatMessage>>>
    {
        private readonly DataContext _context = context;

        public async Task<Response<List<ChatMessage>>> Handle(GetChatMessages request, CancellationToken cancellationToken)
        {
            var response = new Response<List<ChatMessage>>();

            var chatRoom = await _context.ChatRooms
                .Include(cr => cr.Messages)
                .Include(cr => cr.Participants)
                .FirstOrDefaultAsync(cr => cr.Id == request.RoomId, cancellationToken);

            if (chatRoom is null)
            {
                response.AddError(StatusCodes.ChatRoomNotFound, string.Format(ChatErrorMessages.ChatRoomNotFound, request.RoomId));
                return response;
            }

            var isParticipant = chatRoom.Participants.Any(p => p.UserProfileId == request.UserProfileId);
            if (!isParticipant)
            {
                response.AddError(StatusCodes.ChatRoomNotAccessible, ChatErrorMessages.ChatRoomAccessDenied);
                return response;
            }

            response.Payload = chatRoom.Messages.ToList();

            return response;
        }
    }


}
