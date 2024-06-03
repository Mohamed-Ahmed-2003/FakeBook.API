using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakebook.Application.Profile.Queries
{
    public class GetUserProfileByIdQuery(Guid id) : IRequest<Response<UserProfile>>
    {
        public Guid UserId { get; set; } = id;
    }
}
