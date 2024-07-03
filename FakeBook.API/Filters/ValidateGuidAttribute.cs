using Fakebook.Application.Generics.Enums;
using FakeBook.API.Contracts.Others;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FakeBook.API.Filters
{
    public class ValidateGuidAttribute (string key) : ActionFilterAttribute
    {
        private readonly string _key = key;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.TryGetValue(_key, out var value)) return;

            if (Guid.TryParse(value?.ToString(),out var guid)) return;
            var apiError = new ErrorResponse
            {
                StatusCode = (int)StatusCode.NotFound,
                StatusName = "Bad Request",
                Timestamp = DateTime.UtcNow
            };
            apiError.Errors.Add($"This value ' {value} ' is invalid Guid format");
            context.Result = new JsonResult(apiError) { StatusCode = (int)StatusCode.NotFound};
        }
    }
}
