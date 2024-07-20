using AutoMapper;
using Fakebook.Application.CQRS.Account.Commands;
using Fakebook.Application.CQRS.Account.Dtos;
using FakeBook.API.Contracts.Identity.Requests;
using FakeBook.API.Contracts.Identity.Responses;

namespace FakeBook.API.Mapping
{
    public class IdentityMapping : Profile
    {
        public IdentityMapping() {

            CreateMap<UserRegister, UserRegisterCmd>();
            CreateMap<UserLogin, UserLoginCmd>();
            CreateMap<IdentityUserProfileDto, IdentityUserProfile>();
            CreateMap<UserUpdate, UpdateUserCmd>();

        }
    }
}
