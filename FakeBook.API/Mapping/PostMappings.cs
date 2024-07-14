using AutoMapper;
using FakeBook.API.Contracts.Posts.Responses;
using FakeBook.Domain.Aggregates.PostAggregate;

namespace FakeBook.API.Mapping
{
    public class PostMappings : Profile
    {
       public PostMappings ()
        {
            CreateMap<Post, AbstractPost>();
            CreateMap<PostComment, AbstractPostComment>();
            CreateMap<PostInteraction, AbstractPostInteraction>()
                .ForMember(dest => dest.Type, par => par.MapFrom(src => src.Reaction.ToString()))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.UserProfile));
        }
    }
}
