using AutoMapper;
using FakeBook.API.Contracts.UserProfile.Responses;
using FakeBook.Domain.Aggregates.UserProfileAggregate;

namespace FakeBook.API.Mapping
{
    public class UserProfileMappings : Profile
    {
        public UserProfileMappings() {

            CreateMap<UserProfile, UserProfileResponse>();
            CreateMap<GeneralInfo, GeneralInfoResponse>();
        
        }
    }
}
