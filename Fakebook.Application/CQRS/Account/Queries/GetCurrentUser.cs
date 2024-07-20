using Fakebook.Application.CQRS.Account.Dtos;
using Fakebook.Application.Generics;
using MediatR;

namespace Fakebook.Application.Identity.Queries;

public class GetCurrentUser : IRequest<Response<IdentityUserProfileDto>>
{
    public Guid UserProfileId { get; set; }
}