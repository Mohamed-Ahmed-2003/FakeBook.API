using Fakebook.Application.Profile.Commands;
using Fakebook.DAL;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakebook.Application.Profile.CommandHandlers
{
    public class DeleteUserProfileCmdHandler(DataContext context) : IRequestHandler<DeleteUserProfileCmd>
    {
        private readonly DataContext _context = context;

        public async Task Handle(DeleteUserProfileCmd request, CancellationToken cancellationToken)
        {
            var userProfile = await _context.Set<UserProfile>().FindAsync(request.UserProfileId, cancellationToken);

            if (userProfile != null)
            {
            _context.Set<UserProfile>().Remove(userProfile);
            await _context.SaveChangesAsync(cancellationToken);
            }

            return; 
        }
    }

}
