using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DomainObjects.Authentication
{
    using System.ComponentModel;

    using Wams.DataObjects;

    public class ChangePasswordRequest
    {
        public string AccountKey { get; set; }

        [DisplayName("Current password")]
        public string OldPassword { get; set; }

        [DisplayName("New password")]
        public string NewPassword { get; set; }

        [DisplayName("Confirm new password")]
        public string ConfirmNewPassword { get; set; }
    }
}
