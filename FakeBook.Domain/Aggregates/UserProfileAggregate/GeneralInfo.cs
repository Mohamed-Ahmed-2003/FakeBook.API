using FakeBook.Domain.Constants;
using FakeBook.Domain.ValidationExceptions;
using FakeBook.Domain.Validators.ProfileValidators;


namespace FakeBook.Domain.Aggregates.UserProfileAggregate
{
    public class GeneralInfo
    {
        private GeneralInfo() { }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string Phone { get; private set; }
        public string City { get; private set; }

        public static GeneralInfo CreateBasicInfo(string firstName, string lastName, string emailAddress,
      string phone, DateTime dateOfBirth, string city)
        {
            var info = new GeneralInfo
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                Phone = phone,
                DateOfBirth = dateOfBirth,
                City = city
            };

            var validator = new GeneralInfoValidator();
            var validationResult = validator.Validate(info);

            if (!validationResult.IsValid)
            {
                var ex = new ProfileNotValidException(Helper.ExceptionsMessages.ProfileNotValidException);
                foreach (var error in validationResult.Errors)
                {
                    ex.ValidationErrors.Add(error.ErrorMessage);
                }
                throw ex;
            }

            return info;
        }
    }
}
