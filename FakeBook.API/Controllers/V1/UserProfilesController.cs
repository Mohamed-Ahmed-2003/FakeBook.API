using Asp.Versioning;
using AutoMapper;
using Fakebook.Application.Generics;
using Fakebook.Application.Profile.Commands;
using Fakebook.Application.Profile.Queries;
using FakeBook.API.Contracts.Others;
using FakeBook.API.Contracts.UserProfile.Responses;
using FakeBook.API.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FakeBook.API.Controllers.V1
{

    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [Authorize]
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
        //[HttpPost]
        //[ValidateModel]
        //public async Task<IActionResult> CreateProfile([FromBody] UserProfileCreateUpdate userProfileCreate)
        //{
        //    var cmd = _mapper.Map<PostUserProfileCmd>(userProfileCreate);
        //    var userProfile = await _mediator.Send(cmd);
        //    var res =  _mapper.Map<UserProfileResponse>(userProfile);

        //    return CreatedAtAction(nameof(GetUserProfileById), new { id = res.UserProfileId }, res);
        // }

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

      

        //[HttpPatch]
        //[ValidateModel]

        //[Route(ApiRoutes.UserProfile.RouteId)]
        //public async Task<IActionResult> UpdateUserProfile(string id, UserProfileCreateUpdate userProfile)
        //{
            

        //    var cmd = _mapper.Map<PostUserProfileCmd>(userProfile);
        //    cmd.UserProfileId = Guid.Parse(id);

        //    var res = await _mediator.Send(cmd);

        //    if (!res.Success)
        //        return HandleErrorResponse(res.Errors);

        //    var profile = _mapper.Map<UserProfileResponse>(res.Payload);
        //    return Ok(profile);
        //}



    }
}
