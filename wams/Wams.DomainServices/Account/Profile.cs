using System;
using System.ComponentModel;

namespace Wams.ViewModels.Account
{
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
        public string DateOfBirth { get; set; }

        [DisplayName("Email address")]
        public string EmailAddress { get; set; }

        [DisplayName("Telephone")]
        public string Telephone { get; set; }

        [DisplayName("About you")]
        public string Biography { get; set; }

        [DisplayName("Emergency Telephone")]
        public string EmergencyTel { get; set; }
        
        [DisplayName("Membership Type ")]
        public string MembershipType { get; set; }

        [DisplayName("Login Level")]
        public int UserLoginRole { get; set; }

        [DisplayName("Enable Investment")]
        public bool CanInvest { get; set; }
    }
}
