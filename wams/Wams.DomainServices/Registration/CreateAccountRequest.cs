using System;
using System.Runtime.Serialization;


namespace Wams.DomainObjects.Registration
{
    public class CreateAccountRequest
    {
        public string Gender { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }
    }
}
