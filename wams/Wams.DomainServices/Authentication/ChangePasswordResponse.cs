using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DomainObjects.Authentication
{
    using Wams.Enums.Authentication;

    [DataContract]
    public class ChangePasswordResponse
    {
        public AuthenticationStatus Status { get; set; }
    }
}
