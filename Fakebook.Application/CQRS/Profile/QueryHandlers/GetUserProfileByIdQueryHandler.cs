using Fakebook.Application.CQRS.Profile.Queries;
using Fakebook.Application.Generics;
using Fakebook.Application.Generics.Enums;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;


namespace Fakebook.Application.CQRS.Profile.QueryHandlers
{
    public class GetUserProfileByIdQueryHandler(DataContext context) : IRequestHandler<GetUserProfileByIdQuery, Response<UserProfile>>
    {
        private readonly DataContext _context = context;

        public async Task<Response<UserProfile>> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<UserProfile>();

            var profile = await _context.Set<UserProfile>().FindAsync(request.UserId, cancellationToken);

            if (profile is null)
            {
                response.Errors.Add(new ErrorResult { Status = StatusCodes.NotFound, Message = "UserProfile is not exist" });
                return response;
            }

            response.Payload = profile;

            return response;

        }
    }
}
