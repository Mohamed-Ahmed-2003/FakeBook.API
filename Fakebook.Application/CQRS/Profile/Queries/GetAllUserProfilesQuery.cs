﻿using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakebook.Application.CQRS.Profile.Queries
{
    public class GetAllUserProfilesQuery : IRequest<Response<IEnumerable<UserProfile>>>
    {

    }
}
