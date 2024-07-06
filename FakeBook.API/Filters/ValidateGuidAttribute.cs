using Fakebook.Application.Generics.Enums;
using FakeBook.API.Contracts.Others;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FakeBook.API.Filters
{
    public class ValidateGuidAttribute (params string [] keys) : ActionFilterAttribute
    {
        private readonly List<string> _keys = new List<string>(keys);

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var errors = new List<string>();

            foreach (var key in _keys)
            {
                if (!context.ActionArguments.TryGetValue(key, out var value) || !Guid.TryParse(value?.ToString(), out _))
                {
                    errors.Add($"The value for '{key}' is not a valid GUID format.");
                }
            }

            if (errors.Count > 0)
            {
                var apiError = new ErrorResponse
                {
                    StatusCode = (int)StatusCode.NotFound,
                    StatusName = "Bad Request",
                    Timestamp = DateTime.UtcNow,
                    Errors = errors
                };

                context.Result = new JsonResult(apiError) { StatusCode = (int)StatusCode.NotFound };
            }
        }
    }
}
