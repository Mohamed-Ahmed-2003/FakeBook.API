using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.Application.Posts.Queries;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Fakebook.Application.Posts.QueryHandlers
{

    public class GetPostInteractionsHandler(DataContext context) : IRequestHandler<GetPostInteractions, Response<List<PostInteraction>>>
    {
        private readonly DataContext _context = context;

        public async Task<Response<List<PostInteraction>>> Handle(GetPostInteractions request,
            CancellationToken cancellationToken)
        {
            var result = new Response<List<PostInteraction>>();

            try
            {
                var post = await _context.Posts
                    .Include(p => p.Interactions)
                    .ThenInclude(i => i.UserProfile)
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

                if (post is null)
                {
                    result.AddError(StatusCode.NotFound, PostsErrorMessages.PostNotFound);
                    return result;
                }

                result.Payload = post.Interactions.ToList();

            }
            catch (Exception e)
            {
                result.AddError(StatusCode.Unknown,e.Message);
            }

            return result;
        }
    }
}
