using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DomainObjects.Authentication
{
    public class LoginResponse : AuthenticationResponse
    {
        public string Gender { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string EmailAddress { get; set; }
    }
}
