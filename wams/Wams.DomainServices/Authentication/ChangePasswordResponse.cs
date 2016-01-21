using System.Runtime.Serialization;
using Wams.Enums.Authentication;

namespace Wams.ViewModels.Authentication
{
    [DataContract]
    public class ChangePasswordResponse
    {
        public AuthenticationStatus Status { get; set; }
    }
}
