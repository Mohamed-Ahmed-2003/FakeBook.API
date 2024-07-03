using FakeBook.API.Contracts.Others;
using Fakebook.Application.Generics;
using Microsoft.AspNetCore.Mvc;

namespace FakeBook.API.Controllers.V1
{
    public class BaseController : Controller
    {
        protected IActionResult HandleErrorResponse(List<ErrorResult> errors)
        {
            var errRes = new ErrorResponse
            {
                StatusCode = (int)errors[0].Status,
                Errors = errors.Select(res => res.Message).ToList(),
                StatusName = errors[0].Status.ToString(),
                Timestamp = DateTime.Now
            };
            return NotFound(errRes);
        }
    }
}
