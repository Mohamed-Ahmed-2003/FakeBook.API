using Fakebook.Application.Profile.Commands;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Fakebook.Application.Profile.CommandHandlers
{
    public class PostUserProfileCmdHanlder(DataContext context) : IRequestHandler<PostUserProfileCmd, UserProfile>
    {
        private readonly DataContext _context = context;

        public async Task<UserProfile> Handle(PostUserProfileCmd request, CancellationToken cancellationToken)
        {
            UserProfile existed = null;
            if (request.UserProfileId != default)
            {
                existed = await _context.Set<UserProfile>().FindAsync(request.UserProfileId, cancellationToken);

                if (existed is null)
                    return null;
            }

            var info = GeneralInfo.CreateBasicInfo(
                request.FirstName,
                request.LastName,
                request.EmailAddress,
                request.Phone,
                request.DateOfBirth,
                request.City
            );

            if (existed == null)
            {
                var newUserProfile = UserProfile.CreateUserProfile(Guid.NewGuid().ToString(), info);
                await _context.Set<UserProfile>().AddAsync(newUserProfile, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return newUserProfile;
            }
            else
            {
                existed.UpdateBasicInfo(info);
                _context.Set<UserProfile>().Update(existed);
                await _context.SaveChangesAsync(cancellationToken);
                return existed;
            }
        }

    }
}
