using AutoMapper;
using FakeBook.API.Contracts.Posts.Responses;
using FakeBook.API.Contracts.UserProfile.Responses;
using FakeBook.Domain.Aggregates.UserProfileAggregate;

namespace FakeBook.API.Mapping
{
    public class UserProfileMappings : Profile
    {
        public UserProfileMappings() {

            CreateMap<UserProfile, UserProfileResponse>();
            CreateMap<GeneralInfo, GeneralInfoResponse>();
            CreateMap<UserProfile, InteractionAuthor>()
                .ForMember(p => p.FullName,
                opt => opt.MapFrom(s => string.Concat(s.GeneralInfo.FirstName, " ", s.GeneralInfo.LastName)))
                .ForMember(p => p.City, opt => opt.MapFrom(s => s.GeneralInfo.City));

        }
    }
}
