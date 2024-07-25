using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.Application.Generics.Interfaces;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.ChatRoomAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Fakebook.Application.CQRS.Chat.Commands
{
    public class UpdateChatMessageCmd : IRequest<Response<Unit>>
    {
        public Guid RoomId { get; set; }
        public Guid MessageId { get; set; }
        public required string NewContent { get; set; }
    }
    public class UpdateChatMessageCmdHandler(DataContext context , IChatNotifier chatNotifier) : IRequestHandler<UpdateChatMessageCmd, Response<Unit>>
    {
        private readonly DataContext _context = context;
        private readonly IChatNotifier _chatNotifier = chatNotifier;

        public async Task<Response<Unit>> Handle(UpdateChatMessageCmd request, CancellationToken cancellationToken)
        {
            var response = new Response<Unit>();

            var chatRoom = await _context.ChatRooms
                .Include(cr => cr.Messages)
                .FirstOrDefaultAsync(cr => cr.Id == request.RoomId, cancellationToken);

            if (chatRoom is null)
            {
                response.AddError(StatusCodes.ChatRoomNotFound, string.Format(ChatErrorMessages.ChatRoomNotFound, request.RoomId));
                return response;
            }

            var message = chatRoom.Messages.FirstOrDefault(m => m.Id == request.MessageId);

            if (message is null)
            {
                response.AddError(StatusCodes.ChatMessageNotFound, string.Format(ChatErrorMessages.ChatMessageNotFound, request.MessageId));
                return response;
            }

            message.UpdateContent(request.NewContent);

            try
            {
                _context.Set<ChatRoom>().Update(chatRoom);
                await _context.SaveChangesAsync(cancellationToken);
                await _chatNotifier.NotifyMessageUpdated(message);
            }
            catch (Exception ex)
            {
                response.AddError(StatusCodes.ChatMessageUpdateFailed, ChatErrorMessages.ChatMessageUpdateFailed);
                return response;
            }

            response.Payload = Unit.Value; // Indicate that the update was successful
            return response;
        }
    }


}
