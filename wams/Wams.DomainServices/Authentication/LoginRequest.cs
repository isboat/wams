using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DomainObjects.Authentication
{
    using System.ComponentModel.DataAnnotations;

    using Wams.DataObjects;

    /// <summary>
    /// login request
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Username
        /// </summary>
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public Source WebSource { get; set; }
    }
}
