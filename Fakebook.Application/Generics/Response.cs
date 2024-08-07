﻿using Azure;
using Fakebook.Application.Generics.Enums;


namespace Fakebook.Application.Generics
{
    public class Response <T>  
    {
        public bool Success { get; private set; } = true;
        public List<ErrorResult> Errors { get; set; } = new List<ErrorResult>();
        public T? Payload {  get; set; } 

        public void AddError (StatusCodes statusCode , string mesg)
        {
            Success = false;
            Errors.Add (new ErrorResult { Status = statusCode , Message = mesg });
        }
    }
}
