using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.ChatRoomAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.CQRS.Chat.Queries
{
    public class GetChatRoomsQuery : IRequest<Response<List<ChatRoom>>>
    {
        public Guid UserProfileId { get; set; }
    }
    public class GetChatRoomsQueryHandler(DataContext context , IMediator mediator) : IRequestHandler<GetChatRoomsQuery, Response<List<ChatRoom>>>
    {
        private readonly DataContext _context = context;

        public async Task<Response<List<ChatRoom>>> Handle(GetChatRoomsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<ChatRoom>>();

            try
            {
                var chatRooms = await _context.ChatRooms
                    .Include(cr => cr.Participants)
                    .Include(cr => cr.Messages.First())
                    .Where(cr => cr.Participants.Any(p => p.UserProfileId == request.UserProfileId))
                    .ToListAsync(cancellationToken);

                if (chatRooms is null || !chatRooms.Any())
                {
                    response.AddError(StatusCodes.ChatRoomNotFound, ChatErrorMessages.ChatRoomNotFound);
                    return response;
                }
                
                response.Payload = chatRooms;
            }
            catch (Exception ex)
            {
                response.AddError(StatusCodes.DatabaseOperationException, "An error occurred while fetching chat rooms.");
            }

            return response;
        }
    }


}
