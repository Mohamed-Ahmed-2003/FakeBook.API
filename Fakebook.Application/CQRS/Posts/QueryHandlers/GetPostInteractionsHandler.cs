﻿using Fakebook.Application.CQRS.Posts;
using Fakebook.Application.CQRS.Posts.Queries;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Fakebook.Application.CQRS.Posts.QueryHandlers
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
                    result.AddError(StatusCodes.NotFound, PostsErrorMessages.PostNotFound);
                    return result;
                }

                result.Payload = post.Interactions.ToList();

            }
            catch (Exception e)
            {
                result.AddError(StatusCodes.UnknownError, e.Message);
            }

            return result;
        }
    }
}
