﻿using Fakebook.Application.CQRS.Posts;
using Fakebook.Application.CQRS.Posts.Commands;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.PostAggregate;
using FakeBook.Domain.ValidationExceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.CQRS.Posts.CommandHandlers;

public class AddPostCommentHandler(DataContext ctx) : IRequestHandler<AddPostCommentCmd, Response<PostComment>>
{
    private readonly DataContext _ctx = ctx;

    public async Task<Response<PostComment>> Handle(AddPostCommentCmd request, CancellationToken cancellationToken)
    {
        var result = new Response<PostComment>();

        try
        {
            var post = await _ctx.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId,
                cancellationToken: cancellationToken);
            if (post is null)
            {
                result.AddError(StatusCodes.NotFound,
                    string.Format(PostsErrorMessages.PostNotFound, request.PostId));
                return result;
            }

            var comment = PostComment.CreatePostComment(request.UserProfileId, request.CommentText, request.PostId);

            post.AddComment(comment);

            _ctx.Posts.Update(post);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = comment;

        }

        catch (PostCommentNotValidException e)
        {
            e.ValidationErrors.ForEach(er => result.AddError(StatusCodes.ValidationError, er));
        }

        catch (Exception e)
        {
            result.AddError(StatusCodes.UnknownError, e.Message);
        }

        return result;
    }
}