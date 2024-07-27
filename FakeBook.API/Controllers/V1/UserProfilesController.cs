using Asp.Versioning;
using AutoMapper;
using Fakebook.Application.CQRS.Profile.Commands;
using Fakebook.Application.CQRS.Profile.Queries;
using FakeBook.API.Contracts.UserProfile.Responses;
using FakeBook.API.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FakeBook.API.Controllers.V1
{

    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    //[Authorize]
    [ApiController]
    public class UserProfilesController(IMapper mapper,IMediator mediator) : BaseController
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAllUserProfiles ()
        {

            var userProfiles = await _mediator.Send(new GetAllUserProfilesQuery());
            var res =  _mapper.Map<List<UserProfileResponse>>(userProfiles.Payload);
            return Ok(res);
        }

        [HttpGet]
        [Route(ApiRoutes.UserProfile.Search)]
        public async Task<IActionResult> SearchUserProfiles([FromQuery] string query)
        {
            var searchQuery = new SearchUserProfilesQuery
            {
                SearchTerm = query,
            };

            var response = await _mediator.Send(searchQuery);

            if (!response.Success)
                return HandleErrorResponse(response.Errors);

            var profiles = _mapper.Map<List<UserProfileResponse>>(response.Payload);
            return Ok(profiles);
        }



        [HttpGet]
        [Route(ApiRoutes.UserProfile.RouteId)]
        public  async Task <IActionResult> GetUserProfileById(string id)
        {
            var query = new GetUserProfileByIdQuery(Guid.Parse(id));

            var response = await _mediator.Send(query);

            if (!response.Success)
                    return HandleErrorResponse(response.Errors);

            var profile = _mapper.Map<UserProfileResponse>(response.Payload);
       
            return Ok(profile);
        }

        [HttpPost]
        [Route(ApiRoutes.UserProfile.SetProfilePicture)]
        public async Task<IActionResult> SetProfilePicture([FromForm] IFormFile file)
        {
            var userProfileId = HttpContext.User.GetUserProfileId();

            var command = new SetProfilePictureCmd
            {
                UserProfileId = userProfileId,
                FormFile = file
            };

            var response = await _mediator.Send(command);

            if (!response.Success)
                return HandleErrorResponse(response.Errors);

            return Ok();
        }

        [HttpPost]
        [Route(ApiRoutes.UserProfile.SetProfileCoverImage)]
        public async Task<IActionResult> SetProfileCoverImage([FromForm] IFormFile file)
        {
            var userProfileId = HttpContext.User.GetUserProfileId();

            var command = new SetProfileCoverImageCmd
            {
                UserProfileId = userProfileId,
                FormFile = file
            };

            var response = await _mediator.Send(command);

            if (!response.Success)
                return HandleErrorResponse(response.Errors);

            return Ok();
        }


    }
}
