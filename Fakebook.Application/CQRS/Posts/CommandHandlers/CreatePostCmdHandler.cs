using Fakebook.Application.CQRS.Posts.Commands;
using Fakebook.Application.Generics;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.PostAggregate;
using FakeBook.Domain.ValidationExceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakebook.Application.CQRS.Posts.CommandHandlers
{
    public class CreatePostCmdHandler(DataContext context) : IRequestHandler<CreatePostCmd, Response<Post>>
    {
        private readonly DataContext _context = context;

        public async Task<Response<Post>> Handle(CreatePostCmd request, CancellationToken cancellationToken)
        {
            var result = new Response<Post>();
            try
            {
                var post = Post.CreatePost(request.UserProfileId, request.Text);
                await _context.Set<Post>().AddAsync(post);
                await _context.SaveChangesAsync();
                result.Payload = post;
            }
            catch (PostNotValidException ex)
            {
                ex.ValidationErrors.ForEach(error =>
                {
                    result.AddError(Generics.Enums.StatusCode.ValidationError, error);
                });
            }
            catch (Exception ex)
            {
                result.AddError(Generics.Enums.StatusCode.Unknown, ex.Message);
            }
            return result;
        }
    }
}
