using Fakebook.Application.Generics.Enums;
using FakeBook.API.Contracts.Others;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FakeBook.API.Filters
{
    public class ExceptionHandler:ExceptionFilterAttribute
    {
        public override void  OnException(ExceptionContext context)
        {
         
                var apiError = new ErrorResponse
                {
                    StatusCode = (int) StatusCode.ServerError,
                    StatusName = "Internal Server Error",
                    Timestamp = DateTime.UtcNow,
                 
                };
                apiError.Errors.Add(context.Exception.Message);
                context.Result = new JsonResult(apiError) { StatusCode = 500};
            

        }
    }
}
