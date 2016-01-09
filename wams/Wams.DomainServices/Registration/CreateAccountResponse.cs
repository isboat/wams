using System;
using System.Runtime.Serialization;

namespace Wams.DomainObjects.Registration
{
    using Wams.Enums.Registration;

    [DataContract]
    public class CreateAccountResponse
    {

        [DataMember(IsRequired = true)]
        public int MemberId { get; set; }
        
        [DataMember(IsRequired = true)]
        public RegistrationStatus Status { get; set; }
        
    }
}
