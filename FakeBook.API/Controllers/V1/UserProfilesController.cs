using Asp.Versioning;
using AutoMapper;
using Fakebook.Application.Profile.Commands;
using Fakebook.Application.Profile.Queries;
using FakeBook.API.Contracts.UserProfile.Requests;
using FakeBook.API.Contracts.UserProfile.Responses;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FakeBook.API.Controllers.V1
{

    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class UserProfilesController(IMapper mapper,IMediator mediator) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAllUserProfiles ()
        {

            var userProfiles = await _mediator.Send(new GetAllUserProfilesQuery());
            var res =  _mapper.Map<List<UserProfileResponse>>(userProfiles);
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] UserProfileCreateUpdate userProfileCreate)
        {
            var cmd = _mapper.Map<PostUserProfileCmd>(userProfileCreate);
            var userProfile = await _mediator.Send(cmd);
            var res =  _mapper.Map<UserProfileResponse>(userProfile);

            return CreatedAtAction(nameof(GetUserProfileById), new { id = res.UserProfileId }, res);
         }

        [HttpGet]
        [Route(ApiRoutes.UserProfile.RouteId)]
        public  async Task <IActionResult> GetUserProfileById(string id)
        {
            var query = new GetUserProfileByIdQuery(Guid.Parse(id));

            var targetProfile = await _mediator.Send(query);
            var res = _mapper.Map<UserProfileResponse>(targetProfile);
            return Ok(res);
        }

        [HttpPatch]
        [Route(ApiRoutes.UserProfile.RouteId)]
        public async Task<IActionResult> UpdateUserProfile(string id, UserProfileCreateUpdate userProfile)
        {
            if (!Guid.TryParse(id, out Guid userProfileId))
            {
                return BadRequest("Invalid user profile ID.");
            }

            var cmd = _mapper.Map<PostUserProfileCmd>(userProfile);
            cmd.UserProfileId = userProfileId;

            var modifiedUser = await _mediator.Send(cmd);

            if (modifiedUser == null)
            {
                return NotFound("User profile not found.");
            }

            var res = _mapper.Map<UserProfileResponse>(modifiedUser);
            return Ok(res);
        }

        [HttpDelete]
        [Route(ApiRoutes.UserProfile.RouteId)]
        public async Task<IActionResult> DeleteUserProfile(string id)
        {
            if (!Guid.TryParse(id, out Guid userProfileId))
            {
                return BadRequest("Invalid user profile ID.");
            }

            var cmd = new DeleteUserProfileCmd(userProfileId);
            await _mediator.Send(cmd);

            return NoContent();
        }

    }
}
