using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.Application.Posts.Commands;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.Posts.CommandHandlers;

public class DeletePostCmdHandler : IRequestHandler<DeletePostCmd, Response<Post>>
{
    private readonly DataContext _ctx;

    public DeletePostCmdHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<Response<Post>> Handle(DeletePostCmd request, CancellationToken cancellationToken)
    {
        var result = new Response<Post>();
        try
        {
            var post = await _ctx.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken: cancellationToken);
            
            if (post is null)
            {
                result.AddError(StatusCode.NotFound, 
                    string.Format(PostsErrorMessages.PostNotFound, request.PostId));
                
                return result;
            }

            if (post.UserProfileId != request.UserProfileId)
            {
                result.AddError(StatusCode.PostRemovalNotAuthorized , PostsErrorMessages.PostDeleteNotPossible);
                return result;
            }

            _ctx.Posts.Remove(post);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = post;
        }
        catch (Exception e)
        {
            result.AddError(StatusCode.Unknown, e.Message);
        }

        return result;
    }
}