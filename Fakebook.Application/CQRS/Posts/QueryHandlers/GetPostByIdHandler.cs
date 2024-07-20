using Fakebook.Application.CQRS.Posts;
using Fakebook.Application.CQRS.Posts.Queries;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Fakebook.Application.CQRS.Posts.QueryHandlers
{


    public class GetPostByIdHandler(DataContext context) : IRequestHandler<GetPostById, Response<Post>>
    {
        private readonly DataContext _context = context;

        public async Task<Response<Post>> Handle(GetPostById request, CancellationToken cancellationToken)
        {
            var result = new Response<Post>();
            var post = await _context.Posts
                .FirstOrDefaultAsync(p => p.PostId == request.PostId);

            if (post is null)
            {
                result.AddError(StatusCode.NotFound,
                    string.Format(PostsErrorMessages.PostNotFound, request.PostId));
                return result;
            }

            result.Payload = post;
            return result;
        }
    }
}
