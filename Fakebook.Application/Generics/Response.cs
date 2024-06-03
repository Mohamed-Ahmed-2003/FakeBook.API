using Azure;
using Fakebook.Application.Generics.Enums;


namespace Fakebook.Application.Generics
{
    public class Response <T>  where T : class
    {
        public bool Success { get; set; } = true;
        public List<ErrorResult> Errors { get; set; } = new List<ErrorResult>();
        public T? Payload {  get; set; } 
    }
}
