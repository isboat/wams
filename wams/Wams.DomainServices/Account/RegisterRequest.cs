using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Wams.ViewModels.Account
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [DisplayName("First name")]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [DisplayName("Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [DisplayName("Confirm password")]
        [Required(ErrorMessage = "Confirm password is required")]
        public string ConfirmPassword { get; set; }
    }
}
