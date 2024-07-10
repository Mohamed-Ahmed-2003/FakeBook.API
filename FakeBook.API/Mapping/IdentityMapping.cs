using AutoMapper;
using Fakebook.Application.Identity.Commands;
using FakeBook.API.Contracts.Identity.Requests;

namespace FakeBook.API.Mapping
{
    public class IdentityMapping : Profile
    {
        public IdentityMapping() {

            CreateMap<UserRegister, UserRegisterCmd>();
            CreateMap<UserLogin, UserLoginCmd>();

        }
    }
}
