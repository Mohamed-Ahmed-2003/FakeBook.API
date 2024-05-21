using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static GeneralInfo CreateBasicInfo(string FirstName, string lastName, string emailAddress,
          string phone, DateTime dateOfBirth, string city)
        {

            return new GeneralInfo
            {
                FirstName = FirstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                Phone = phone,
                DateOfBirth = dateOfBirth,
                City = city
            };
        }
    }
}
