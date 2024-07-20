﻿using Fakebook.Application.CQRS.Posts;
using Fakebook.Application.CQRS.Posts.Commands;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.CQRS.Posts.CommandHandlers;

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
                result.AddError(StatusCode.PostDeleteNotPossible, PostsErrorMessages.PostDeleteNotPossible);
                return result;
            }

            _ctx.Posts.Remove(post);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = post;
        }
        catch (Exception e)
        {
            result.AddError(StatusCode.UnknownError, e.Message);
        }

        return result;
    }
}