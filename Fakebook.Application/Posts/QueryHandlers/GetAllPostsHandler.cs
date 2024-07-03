
using Fakebook.Application.Generics;
using Fakebook.Application.Posts.Queries;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.Posts.QueryHandlers
{
    public class GetAllPostsHandler(DataContext context) : IRequestHandler<GetAllPosts, Response<List<Post>>>
    {
        private readonly DataContext _context = context;
        public async Task<Response<List<Post>>> Handle(GetAllPosts request, CancellationToken cancellationToken)
        {
            var result = new Response<List<Post>>();

            try
            {
            var posts = await _context.Posts.ToListAsync();
                result.Payload = posts;
            }
            catch (Exception ex)
            {
                result.Success = false;
                var err = new ErrorResult { Status = Generics.Enums.StatusCode.Unknown, Message = ex.Message };
                result.Errors.Add(err);
            }
           
                return result;
            
        }
    }
}
