using Asp.Versioning;
using AutoMapper;
using Fakebook.Application.Posts.Queries;
using FakeBook.API.Contracts.Posts.Responses;
using FakeBook.API.Filters;
using FakeBook.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FakeBook.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class PostsController(IMapper mapper, IMediator mediator) : BaseController
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [Route(ApiRoutes.Post.RouteId)]
        [ValidateGuid("id")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var queryResult = await _mediator.Send(new GetPostById { PostId = Guid.Parse(id)});

            if (!queryResult.Success)
            {
                return HandleErrorResponse(queryResult.Errors);
            }
            var response = _mapper.Map<AbstractPostResponse>(queryResult.Payload);

            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll ()
        {
            var queryResult = await _mediator.Send(new GetAllPosts());

            if (!queryResult.Success ) {
                return HandleErrorResponse(queryResult.Errors);
            }
            var response = _mapper.Map<List<AbstractPostResponse>>(queryResult.Payload);

            return Ok(response);
        }
    }
}
