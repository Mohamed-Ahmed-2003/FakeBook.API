using Fakebook.Application.Generics.Enums;

namespace Fakebook.Application.Generics
{
    public class ErrorResult
    {
        public StatusCode Status { get; set; }
        public string? Message { get; set; }
    }
}
