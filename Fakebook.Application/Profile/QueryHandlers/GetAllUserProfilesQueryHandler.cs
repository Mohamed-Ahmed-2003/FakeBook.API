using Fakebook.Application.Generics;
using Fakebook.Application.Profile.Queries;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.Profile.QueryHandlers
{
    public class GetAllUserProfilesQueryHandler (DataContext context) : IRequestHandler<GetAllUserProfilesQuery, Response<IEnumerable<UserProfile>>>
    {
        private readonly DataContext _context = context;

        public async Task<Response<IEnumerable<UserProfile>>> Handle(GetAllUserProfilesQuery request, CancellationToken cancellationToken)
        {

            var response = new Response<IEnumerable<UserProfile>>();
            response.Payload =  await _context.Set<UserProfile>().ToListAsync(cancellationToken);

            return response;

        }
    }
}
