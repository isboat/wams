using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wams.Enums.Authentication;

namespace Wams.DomainObjects.Authentication
{
    public class AuthenticationResponse
    {

        /// <summary>
        /// Account key of logged in user
        /// </summary>
        [DataMember]
        public string AccountKey { get; set; }

        /// <summary>
        /// Status of login, whether login was successful or an error occured
        /// </summary>
        [DataMember]
        public AuthenticationStatus AuthenticationStatus { get; set; }

        /// <summary>
        /// Login Message
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        public string FormsAuthCookieName { get; set; }

        public string FormsAuthCookieValue { get; set; }
    }
}
