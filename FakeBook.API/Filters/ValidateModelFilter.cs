using FakeBook.API.Contracts.Others;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FakeBook.API.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var apiError = new ErrorResponse
                {
                    StatusCode = 400,
                    StatusName = "Bad Request",
                    Timestamp = DateTime.UtcNow,
                    Errors = context.ModelState.Values.SelectMany(v => v.Errors).Select (e=>e.ErrorMessage).ToList(),
                };
                context.Result = new JsonResult(apiError) { StatusCode = 400};
            }
        }
    }
}
