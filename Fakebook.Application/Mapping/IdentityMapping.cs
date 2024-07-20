using AutoMapper;
using Fakebook.Application.CQRS.Account.Dtos;
using FakeBook.Domain.Aggregates.UserProfileAggregate;


namespace Fakebook.Application.Mapping
{
    public class IdentityMapping :Profile
    {
        public IdentityMapping()
        {
            CreateMap<UserProfile, IdentityUserProfileDto>()
                .ForMember(p => p.FirstName, opt => opt.MapFrom(src => src.GeneralInfo.FirstName))
                .ForMember(p => p.LastName, opt => opt.MapFrom(src => src.GeneralInfo.LastName))
                .ForMember(p => p.EmailAddress, opt => opt.MapFrom(src => src.GeneralInfo.EmailAddress))
                .ForMember(p => p.CurrentCity, opt => opt.MapFrom(src => src.GeneralInfo.City))
                .ForMember(p => p.DateOfBirth, opt => opt.MapFrom(src => src.GeneralInfo.DateOfBirth))
                .ForMember(p => p.Phone, opt => opt.MapFrom(src => src.GeneralInfo.Phone));
        }
    }
}
