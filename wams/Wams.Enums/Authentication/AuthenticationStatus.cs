using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.Enums.Authentication
{
    public enum AuthenticationStatus
    {
        /// <summary>
        /// Login was successful
        /// </summary>
        Successful,

        /// <summary>
        /// The login failed
        /// </summary>
        Failed,

        /// <summary>
        /// Login failed due to the username entered
        /// </summary>
        UsernameError,

        /// <summary>
        /// Login failed due to the password entered
        /// </summary>
        PasswordError,

        /// <summary>
        /// Login failed because the user's account has been suspended
        /// </summary>
        AccountSuspended
    }
}
