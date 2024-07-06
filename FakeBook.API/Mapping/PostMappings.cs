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
        }


    }
}
