using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Wams.ViewModels.Account
{
    public class RegisterRequest
    {
        [DisplayName("First name")]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [DisplayName("Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [DisplayName("Confirm password")]
        [Required(ErrorMessage = "Confirm password is required")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        public IEnumerable<SelectListItem> GenderOptions { get; set; }

        [DisplayName("Date of birth")]
        [Required(ErrorMessage = "Date of birth is required (dd-MM-yyyy)")]
        public string DateOfBirth { get; set; }

        [DisplayName("Address/Location")]
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [DisplayName("Occupation")]
        [Required(ErrorMessage = "Occupation is required")]
        public string Occupation { get; set; }

        [DisplayName("Membership type")]
        [Required(ErrorMessage = "Membership type is required")]
        public string MembershipType { get; set; }

        public IEnumerable<SelectListItem> MembershipTypeOptions { get; set; }
    }
}
