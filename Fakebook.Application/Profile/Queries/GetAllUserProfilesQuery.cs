using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakebook.Application.Profile.Queries
{
    public class GetAllUserProfilesQuery : IRequest<IEnumerable<UserProfile>>
    {

    }
}
