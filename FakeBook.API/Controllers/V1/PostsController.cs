using Asp.Versioning;
using FakeBook.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FakeBook.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class PostsController : ControllerBase
    {
        [HttpGet]
        [Route(ApiRoutes.Post.RouteId)]
        public IActionResult GetById(int id)
        {
                return Ok();
        }
    }
}
