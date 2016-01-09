﻿namespace Wams.DomainObjects.Account
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class Profile
    {
        public int MemberId { get; set; }

        public string ProfilePicUrl { get; set; }

        [DisplayName("Gender")]
        public string Gender { get; set; }

        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DisplayName("Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Email address")]
        public string EmailAddress { get; set; }

        [DisplayName("Telephone")]
        public string Telephone { get; set; }

        [DisplayName("About you")]
        public string Biography { get; set; }

        [DisplayName("Emergency Telephone")]
        public string EmergencyTel { get; set; }
    }
}
