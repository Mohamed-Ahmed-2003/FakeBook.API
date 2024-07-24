using Fakebook.Application.Generics;
using MediatR;

namespace Fakebook.Application.CQRS.Chat.Commands
{
    public class DeleteChatMessageCmd : IRequest<Response<bool>>
    {
        public Guid RoomId { get; set; }
        public Guid MessageId { get; set; }
        public Guid UserProfileId { get; set; }

    }


    public class DeleteChatMessageCmdHandler : IRequestHandler<DeleteChatMessageCmd, Response<bool>>
    {
        public async Task<Response<bool>> Handle(DeleteChatMessageCmd request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();

            // Implementation for deleting a message

            // On success
            response.Payload = true; // Indicate that the deletion was successful

            // On failure
            // response.AddError(StatusCodes.Status400BadRequest, "Error message");

            return response;
        }
    }

}
