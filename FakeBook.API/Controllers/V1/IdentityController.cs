using Asp.Versioning;
using AutoMapper;
using Fakebook.Application.Identity.Commands;
using FakeBook.API.Contracts.Identity.Requests;
using FakeBook.API.Contracts.Identity.Responses;
using FakeBook.API.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FakeBook.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class IdentityController(IMapper mapper, IMediator mediator) : BaseController
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [Route(ApiRoutes.Identity.Register)]
        [ValidateModel]
        public async Task<IActionResult> Register ([FromBody] UserRegister register)
        {
            var cmd = _mapper.Map<UserRegisterCmd>(register);
            var res = await _mediator.Send(cmd);

            if (!res.Success) return HandleErrorResponse(res.Errors);

            var authRes = new AuthResult { Token = res.Payload };

            return Ok(authRes);
        } 
        
        [HttpPost]
        [Route(ApiRoutes.Identity.Login)]
        [ValidateModel]
        public async Task<IActionResult> Login ([FromBody] UserLogin login)
        {
            var cmd = new UserLoginCmd { Username = login.Username , Password  = login.Password};
            var res = await _mediator.Send(cmd);
            if (!res.Success) return HandleErrorResponse(res.Errors);

            var authRes = new AuthResult { Token = res.Payload };

            return Ok(authRes);
        }

    }
}
