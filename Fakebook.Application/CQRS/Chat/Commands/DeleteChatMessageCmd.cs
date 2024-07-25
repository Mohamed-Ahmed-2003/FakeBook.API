using MediatR;
using Microsoft.EntityFrameworkCore;
using Fakebook.DAL;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.Application.CQRS.Chat;
using FakeBook.Domain.Aggregates.ChatRoomAggregate;
using Fakebook.Application.Generics.Interfaces;

public class DeleteChatMessageCmd : IRequest<Response<bool>>
{
    public Guid RoomId { get; set; }
    public Guid MessageId { get; set; }
    public Guid UserProfileId { get; set; }
}

public class DeleteChatMessageCmdHandler(DataContext context , IChatNotifier chatNotifier) : IRequestHandler<DeleteChatMessageCmd, Response<bool>>
{
    private readonly DataContext _context = context;
    private readonly IChatNotifier _chatNotifier = chatNotifier;

    public async Task<Response<bool>> Handle(DeleteChatMessageCmd request, CancellationToken cancellationToken)
    {
        var response = new Response<bool>();

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

        if (message.UserProfileId != request.UserProfileId)
        {
            response.AddError(StatusCodes.ChatMessageDeleteNotAllowed, ChatErrorMessages.ChatMessageDeleteNotAllowed);
            return response;
        }
        chatRoom.RemoveMessage(message);

        try {
            _context.Set<ChatRoom>().Update(chatRoom);
            await _context.SaveChangesAsync(cancellationToken);
            await _chatNotifier.NotifyMessageDeleted(message);
          }
        catch (Exception ex)
        {
            response.AddError(StatusCodes.ChatMessageDeletionFailed, ChatErrorMessages.ChatMessageDeletionFailed);
            return response;
        }

        response.Payload = true; 
        return response;
    }
}
