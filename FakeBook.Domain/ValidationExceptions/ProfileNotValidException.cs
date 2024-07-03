using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeBook.Domain.ValidationExceptions
{
    public class ProfileNotValidException : NotValidException
    {

        public ProfileNotValidException() { }
        public ProfileNotValidException(string mesg) : base(mesg) { }
        public ProfileNotValidException(string mesg, Exception inner) : base(mesg, inner) { }

    }
}
