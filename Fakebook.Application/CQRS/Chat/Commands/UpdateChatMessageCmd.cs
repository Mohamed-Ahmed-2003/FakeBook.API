using Fakebook.Application.Generics;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakebook.Application.CQRS.Chat.Commands
{
    public class UpdateChatMessageCmd : IRequest<Response<Unit>>
    {
        public Guid RoomId { get; set; }
        public Guid MessageId { get; set; }
        public required string NewContent { get; set; }
    }
    public class UpdateChatMessageCmdHandler : IRequestHandler<UpdateChatMessageCmd, Response<Unit>>
    {
        public async Task<Response<Unit>> Handle(UpdateChatMessageCmd request, CancellationToken cancellationToken)
        {
            var response = new Response<Unit>();

            // Implementation for updating a message

            // On success

            // On failure
            // response.AddError(StatusCodes.Status400BadRequest, "Error message");

            return response;
        }
    }

}
