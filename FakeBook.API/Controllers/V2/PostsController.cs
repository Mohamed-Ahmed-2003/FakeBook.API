using Asp.Versioning;
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
            

            return Ok();
        }
    }
}
