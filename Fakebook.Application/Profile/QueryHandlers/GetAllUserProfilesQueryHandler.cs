using Fakebook.Application.Profile.Queries;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.Profile.QueryHandlers
{
    public class GetAllUserProfilesQueryHandler (DataContext context) : IRequestHandler<GetAllUserProfilesQuery, IEnumerable<UserProfile>>
    {
        private readonly DataContext _context = context;

        public async Task<IEnumerable<UserProfile>> Handle(GetAllUserProfilesQuery request, CancellationToken cancellationToken)
        {
            
            return await _context.Set<UserProfile>().ToListAsync(cancellationToken);

        }
    }
}
