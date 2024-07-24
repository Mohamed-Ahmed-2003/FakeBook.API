using Asp.Versioning;
using AutoMapper;
using Fakebook.Application.CQRS.Account.Commands;
using Fakebook.Application.Identity.Queries;
using FakeBook.API.Contracts.Identity.Requests;
using FakeBook.API.Contracts.Identity.Responses;
using FakeBook.API.Extensions;
using FakeBook.API.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FakeBook.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    [Authorize]
    public class AccountController(IMapper mapper, IMediator mediator) : BaseController
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [Route(ApiRoutes.Identity.Register)]
        [ValidateModel]
        [AllowAnonymous]
        public async Task<IActionResult> Register ([FromBody] UserRegister register)
        {
            var cmd = _mapper.Map<UserRegisterCmd>(register);
            var res = await _mediator.Send(cmd);

            if (!res.Success) return HandleErrorResponse(res.Errors);

            var profile = _mapper.Map<IdentityUserProfile>(res.Payload);

            return Ok(profile);
        } 
        
        [HttpPost]
        [Route(ApiRoutes.Identity.Login)]
        [ValidateModel]
        [AllowAnonymous]

        public async Task<IActionResult> Login ([FromBody] UserLogin login)
        {
            var cmd = new UserLoginCmd { Username = login.Username , Password  = login.Password};
            var res = await _mediator.Send(cmd);
            if (!res.Success) return HandleErrorResponse(res.Errors);


            var profile = _mapper.Map<IdentityUserProfile>(res.Payload);

            return Ok(profile);
        }
        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserUpdate userUpdate)
        {
            var userProfileId = HttpContext.User.GetUserProfileId();

            var cmd = _mapper.Map<UpdateUserCmd>(userUpdate);
            cmd.UserProfileId = userProfileId;
            var res = await _mediator.Send(cmd);

            if (!res.Success) return HandleErrorResponse(res.Errors);

            var profile = _mapper.Map<IdentityUserProfile>(res.Payload);

            return Ok(profile);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete ()
        {
            var userProfileId = HttpContext.User.GetUserProfileId();
            var cmd = new DeleteUserCmd
            {
                UserProfileId = userProfileId
            };
            var res = await _mediator.Send(cmd);
            if (!res.Success)
                return HandleErrorResponse(res.Errors);
            return NoContent();
        }

        [HttpGet]
        [Route(ApiRoutes.Identity.CurrentUser)]
        public async Task<IActionResult> CurrentUser(CancellationToken token)
        {
            var userProfileId = HttpContext.User.GetUserProfileId();

            var query = new GetCurrentUser { UserProfileId = userProfileId };
            var result = await _mediator.Send(query, token);
            if (!result.Success) return HandleErrorResponse(result.Errors);
            var mappedProfile = _mapper.Map<IdentityUserProfile>(result.Payload);
            return Ok(mappedProfile);
        }

        [HttpPost]
        [Route(ApiRoutes.Identity.ChangePassword)]
        [ValidateModel]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword request)
        {

            var cmd = new ChangePasswordCmd
            {
                UserId = HttpContext.User.GetIdentityUserId(),
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword
            };

            var res = await _mediator.Send(cmd);

            if (!res.Success) return HandleErrorResponse(res.Errors);

            return NoContent();
        }

        [HttpPost]
        [Route(ApiRoutes.Identity.ForgotPassword)]
        [ValidateModel]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword request)
        {
            var cmd = new ForgotPasswordCmd
            {
                Email = request.Email
            };

            var res = await _mediator.Send(cmd);

            if (!res.Success) return HandleErrorResponse(res.Errors);

            return NoContent();
        }
        [HttpPost]
        [Route(ApiRoutes.Identity.ConfirmEmail)]
        [ValidateModel]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmail request)
        {
            var cmd = new ConfirmEmailCmd
            {
                UserId = HttpContext.User.GetIdentityUserId(),
                Token = request.Token
            };

            var res = await _mediator.Send(cmd);

            if (!res.Success) return HandleErrorResponse(res.Errors);

            return NoContent();
        }

        [HttpGet]
        [Route(ApiRoutes.Identity.ConfirmEmail)]
        [ValidateModel]
        public async Task<IActionResult> SendConfirmEmail ()
        {

            var cmd = new SendConfirmEmailCmd
            {
                  UserId = HttpContext.User.GetIdentityUserId()
            };

            var res = await _mediator.Send(cmd);

            if (!res.Success) return HandleErrorResponse(res.Errors);

            return NoContent();
        }


        [HttpPost]
        [Route(ApiRoutes.Identity.ResetPassword)]
        [ValidateModel]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword request)
        {
            var cmd = new ResetPasswordCmd
            {
                Email = request.Email,
                Token = request.Token,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword
            };

            var response = await _mediator.Send(cmd);

            if (!response.Success)
            {
                return BadRequest(response.Errors);
            }

            return Ok(response.Payload);
        }

     

    }
}
