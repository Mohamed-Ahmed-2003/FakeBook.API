using AutoMapper;
using Fakebook.Application.Profile.Commands;
using FakeBook.API.Contracts.UserProfile.Requests;
using FakeBook.API.Contracts.UserProfile.Responses;
using FakeBook.Domain.Aggregates.UserProfileAggregate;

namespace FakeBook.API.Mapping
{
    public class UserProfileMappings : Profile
    {
        public UserProfileMappings() {

            CreateMap<UserProfileCreateUpdate, PostUserProfileCmd>();
            CreateMap<UserProfile, UserProfileResponse>();
            CreateMap<GeneralInfo, GeneralInfoResponse>();
        
        }
    }
}
