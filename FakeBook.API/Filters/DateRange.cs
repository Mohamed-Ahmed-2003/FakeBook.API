using System.ComponentModel.DataAnnotations;

namespace FakeBook.API.Filters
{
    public class DateOfBirthRangeAttribute : ValidationAttribute
    {
        private readonly int _minAge;
        private readonly int _maxAge;

        public DateOfBirthRangeAttribute(int minAge, int maxAge)
        {
            _minAge = minAge;
            _maxAge = maxAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateOfBirth)
            {
                var minDate = DateTime.Now.AddYears(-_maxAge);
                var maxDate = DateTime.Now.AddYears(-_minAge);

                if (dateOfBirth >= minDate && dateOfBirth <= maxDate)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return new ValidationResult("Invalid date format");
        }

        private string GetErrorMessage()
        {
            return $"Date of Birth must be between {DateTime.Now.AddYears(-_maxAge):yyyy-MM-dd} and {DateTime.Now.AddYears(-_minAge):yyyy-MM-dd}";
        }
    }
}
