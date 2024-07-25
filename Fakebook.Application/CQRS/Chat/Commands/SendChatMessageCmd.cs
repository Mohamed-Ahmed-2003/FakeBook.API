using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.Application.Generics.Interfaces;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.ChatRoomAggregate;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using FakeBook.Domain.ValidationExceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Fakebook.Application.CQRS.Chat.Commands
{
    public class SendChatMessageCmd : IRequest<Response<Unit>>
    {
        public Guid RoomId { get; set; }
        public Guid UserProfileId { get; set; }
        public required string Content { get; set; }
    }
    public class SendChatMessageCmdHandler(IChatNotifier chatNotifier, DataContext context) : IRequestHandler<SendChatMessageCmd, Response<Unit>>
    {
        private readonly IChatNotifier _chatNotifier = chatNotifier;
        private readonly DataContext _context = context;
        public async Task<Response<Unit>> Handle(SendChatMessageCmd request, CancellationToken cancellationToken)
        {
            var response = new Response<Unit>();

            var existedRoom = await _context.ChatRooms.Include(r=>r.Participants)
                .FirstOrDefaultAsync(r=>r.Id == request.RoomId, cancellationToken);
            if (existedRoom is null)
            {
                response.AddError(StatusCodes.NotFound,ChatErrorMessages.ChatRoomNotFound);
                return response;
            }
            if (!existedRoom.Participants.Any(p => p.UserProfileId == request.UserProfileId))
            {
                response.AddError(StatusCodes.ChatRoomNotAccessible, ChatErrorMessages.ChatMessageSendingFailed);
                return response;
            }
            // Implementation for sending a message
            try
            {
                var senderName = (await _context.Set<UserProfile>().FindAsync(request.UserProfileId))?.GetFullName() ?? "Unknown"; 

            var mesg = ChatMessage.CreateMessage(request.RoomId, request.UserProfileId,senderName, request.Content);

            await _chatNotifier.NotifyMessageSent( mesg);

                existedRoom.SendMessage(mesg);
                _context.Set<ChatRoom>().Update(existedRoom);
                await _context.SaveChangesAsync();
            }
            catch (ChatMessageNotValidException ex)
            {
                foreach (var err in ex.ValidationErrors)
                {
                    response.AddError(StatusCodes.ValidationError, err);
                }
            }
            catch (Exception ex)
            {
                response.AddError(StatusCodes.ChatMessageSendingFailed, ChatErrorMessages.ChatMessageSendingFailed);
            }
           return response;
        }
    }

}
