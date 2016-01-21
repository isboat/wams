﻿using System;

namespace Wams.ViewModels.Registration
{
    public class CreateAccountRequest
    {
        public string Gender { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string MembershipType { get; set; }

        public int UserLoginRole { get; set; }
    }
}
