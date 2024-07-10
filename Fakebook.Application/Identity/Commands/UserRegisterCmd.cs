using Fakebook.Application.Generics;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace Fakebook.Application.Identity.Commands
{
    public class UserRegisterCmd : IRequest<Response<string>>
    {
        public  required string Username { get; set; }

        public  required string FirstName { get; set; }

        public  required string LastName { get; set; }

        public  required string Password { get; set; }

        public  required string ConfirmPassword { get; set; }


        public DateTime DateOfBirth { get; set; }
        public required string Phone { get; set; }
        public required string City { get; set; }
    }
}
