using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.Application.Posts.Commands;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.PostAggregate;
using FakeBook.Domain.ValidationExceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.Posts.CommandHandlers;

public class UpdatePostCmdHandler : IRequestHandler<UpdatePostCmd, Response<Post>>
{
    private readonly DataContext _ctx;

    public UpdatePostCmdHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<Response<Post>> Handle(UpdatePostCmd request, CancellationToken cancellationToken)
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

            //if (post.UserProfileId != request.UserProfileId)
            //{
            //    result.AddError(StatusCode.NotFound, PostsErrorMessages.PostUpdateNotPossible);
            //    return result;
            //}
            
            post.UpdatePostText(request.NewText);

            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = post;
        }
        
        catch (PostNotValidException e)
        {
            e.ValidationErrors.ForEach(er => result.AddError(StatusCode.ValidationError, er));
        }
        catch (Exception e)
        {
            result.AddError(StatusCode.Unknown, e.Message);
        }

        return result;
    }

  
}