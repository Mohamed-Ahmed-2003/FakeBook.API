using FakeBook.Domain.Aggregates.UserProfileAggregate;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeBook.Domain.Validators.ProfileValidators
{
    public class GeneralInfoValidator : AbstractValidator<GeneralInfo>
    {
        public GeneralInfoValidator()
        {

            RuleFor(info => info.FirstName)
                .NotNull().WithMessage("First name is required.")
                .MinimumLength(3).WithMessage("First name must contain at least 3 characters.")
                .MaximumLength(50).WithMessage("First name must contain no more than 50 characters.");

            RuleFor(info => info.LastName)
                .NotNull().WithMessage("Last name is required.")
                .MinimumLength(3).WithMessage("Last name must contain at least 3 characters.")
                .MaximumLength(50).WithMessage("Last name must contain no more than 50 characters.");

            RuleFor(info => info.EmailAddress)
                .NotNull().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("Provide a valid email address.");

            RuleFor(info => info.DateOfBirth)
                .InclusiveBetween(DateTime.Now.AddYears(-150), DateTime.Now.AddYears(-18))
                .WithMessage("Age must be between 18 and 150 years.");

        }
    }
}
