using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakebook.Application.CQRS.Posts.Queries
{
    public class GetPostById : IRequest<Response<Post>>
    {
        public Guid PostId { get; set; }
    }
}
