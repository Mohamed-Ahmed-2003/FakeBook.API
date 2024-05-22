using Fakebook.Application.Profile.Queries;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakebook.Application.Profile.QueryHandlers
{
    public class GetUserProfileByIdQueryHandler(DataContext context) : IRequestHandler<GetUserProfileByIdQuery, UserProfile>
    {
        private readonly DataContext _context = context;

        public async Task<UserProfile> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Set<UserProfile>().FindAsync(request.UserId,cancellationToken);
        }
    }
}
