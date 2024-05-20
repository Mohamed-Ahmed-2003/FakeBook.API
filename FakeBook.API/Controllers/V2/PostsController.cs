using Asp.Versioning;
using FakeBook.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FakeBook.API.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("Api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var post = new Post { ID = id, Likes = 1000000, Text = "Al Ahly will draw today. remember", Timestamp = DateTime.Now.AddDays(-1) };

            return Ok(post);
        }
    }
}
