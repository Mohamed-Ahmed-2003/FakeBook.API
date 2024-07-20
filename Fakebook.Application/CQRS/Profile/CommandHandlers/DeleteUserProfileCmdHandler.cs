using Fakebook.Application.CQRS.Profile.Commands;
using Fakebook.Application.Generics;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;


namespace Fakebook.Application.CQRS.Profile.CommandHandlers
{
    public class DeleteUserProfileCmdHandler(DataContext context) : IRequestHandler<DeleteUserProfileCmd, Response<UserProfile>>
    {
        private readonly DataContext _context = context;

        public async Task<Response<UserProfile>> Handle(DeleteUserProfileCmd request, CancellationToken cancellationToken)
        {
            var response = new Response<UserProfile>();

            var userProfile = await _context.Set<UserProfile>().FindAsync(request.UserProfileId, cancellationToken);

            if (userProfile is null)
            {
                response.Errors.Add(new ErrorResult { Status = Generics.Enums.StatusCode.NotFound, Message = "User Profile is not exist" });
            }
            else
            {
                _context.Set<UserProfile>().Remove(userProfile);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return response;
        }
    }

}
