using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.Application.Posts.Queries;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.Posts.QueryHandlers;

public class GetPostCommentsHandler : IRequestHandler<GetPostComments, Response<List<PostComment>>>
{
    private readonly DataContext _ctx;

    public GetPostCommentsHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Response<List<PostComment>>> Handle(GetPostComments request, CancellationToken cancellationToken)
    {
        var result = new Response<List<PostComment>>();

        try
        {
            var post = await _ctx.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

            if (post == null)
            {
                result.AddError(StatusCode.NotFound, string.Format(PostsErrorMessages.PostNotFound, request.PostId));
                return result;
            }

            result.Payload = post.Comments.ToList();
        }
        catch (Exception e)
        {
            result.AddError(StatusCode.Unknown, e.Message);
        }

        return result;
    }
}
