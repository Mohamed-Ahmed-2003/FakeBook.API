using Fakebook.Application.Generics;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Application.CQRS.Profile.Queries
{
    public class SearchUserProfilesQuery : IRequest<Response<List<UserProfile>>>
    {
        public string SearchTerm { get; set; }

      
    }
    public class SearchUserProfilesQueryHandler(DataContext context) : IRequestHandler<SearchUserProfilesQuery, Response<List<UserProfile>>>
    {
        private readonly DataContext _context = context;

        public async Task<Response<List<UserProfile>>> Handle(SearchUserProfilesQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<UserProfile>>();

            try
            {
                var userProfiles = await _context.UserProfiles
                    .Where(up => up.GeneralInfo.FirstName.Contains(request.SearchTerm) ||
                                 up.GeneralInfo.LastName.Contains(request.SearchTerm))
                    .ToListAsync(cancellationToken);


                response.Payload = userProfiles;
            }
            catch (Exception ex)
            {
                response.AddError(Generics.Enums.StatusCodes.UnknownError, "An error occurred: " + ex.Message);
            }

            return response;
        }
    }


}
