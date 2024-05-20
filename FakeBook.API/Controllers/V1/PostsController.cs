using Asp.Versioning;
using FakeBook.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FakeBook.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("Api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var post = new Post { ID = id, Likes = 1000000, Text = "Al Ahly will win today 3 to zero. remember", Timestamp = DateTime.Now.AddDays(-1) };

            return Ok(post);
        }
    }
}
