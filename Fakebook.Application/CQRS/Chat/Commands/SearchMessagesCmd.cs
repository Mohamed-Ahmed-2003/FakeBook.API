

using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.ChatRoomAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.CQRS.Chat.Commands
{
    public class SearchMessagesCmd : IRequest<Response<List<ChatMessage>>>
    {
        public required string SearchTerm { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid UserProfileId { get; set; }
    }
    public class SearchMessagesCmdHandler(DataContext context) : IRequestHandler<SearchMessagesCmd, Response<List<ChatMessage>>>
    {
        private readonly DataContext _context = context;

        public async Task<Response<List<ChatMessage>>> Handle(SearchMessagesCmd request, CancellationToken cancellationToken)
        {
            var response = new Response<List<ChatMessage>>();

            try
            {
                // Retrieve chat rooms where the user is a participant
                var messages = await _context.ChatRooms
                    .Include(cr => cr.Messages)
                    .Where(cr => cr.Participants.Any(p => p.UserProfileId == request.UserProfileId))
                    .SelectMany(cr=>cr.Messages).ToListAsync(cancellationToken);

                // Filter messages based on the search criteria
                var filteredMessages = messages
                    .Where(m => m.Content.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)
                                && m.SentAt >= request.StartDate
                                && m.SentAt <= request.EndDate)
                    .ToList();

                response.Payload = filteredMessages;
            }
            catch (Exception ex)
            {
                response.AddError(StatusCodes.UnknownError, ChatErrorMessages.ChatMessageSearchFailed);
            }

            return response;
        }
    }

}
