using Fakebook.Application.CQRS.Account;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Fakebook.Application.CQRS.Profile.Queries
{
    public class GetProfilesByIds : IRequest<Response<List<UserProfile>>>
    {
        public required Guid[] UsersIds { get; set; }
    }

    public class GetUsersProfilesByIdsHandler : IRequestHandler<GetProfilesByIds, Response<List<UserProfile>>>
    {
        private readonly DataContext _context;

        public GetUsersProfilesByIdsHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<List<UserProfile>>> Handle(GetProfilesByIds request, CancellationToken cancellationToken)
        {
            var response = new Response<List<UserProfile>>();

            if (request.UsersIds == null || !request.UsersIds.Any())
            {
                response.AddError(StatusCode.NotFound, AccountErrorMessages.AccountNotFound);
                return response;
            }

            var profiles = await _context.Set<UserProfile>()
                .Where(p =>  request.UsersIds.Contains(p.UserProfileId))
                .ToListAsync(cancellationToken);

            if (!profiles.Any())
            {
                response.AddError(StatusCode.NotFound, AccountErrorMessages.AccountNotFound);
            }
            else
            {
                response.Payload = profiles;
            }

            return response;
        }
    }
}
