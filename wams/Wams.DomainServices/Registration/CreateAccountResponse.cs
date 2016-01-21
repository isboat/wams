using System.Runtime.Serialization;
using Wams.Enums.Registration;

namespace Wams.ViewModels.Registration
{
    [DataContract]
    public class CreateAccountResponse
    {

        [DataMember(IsRequired = true)]
        public int MemberId { get; set; }
        
        [DataMember(IsRequired = true)]
        public RegistrationStatus Status { get; set; }
        
    }
}
