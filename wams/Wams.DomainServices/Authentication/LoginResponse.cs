namespace Wams.ViewModels.Authentication
{
    public class LoginResponse : AuthenticationResponse
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }
    }
}
