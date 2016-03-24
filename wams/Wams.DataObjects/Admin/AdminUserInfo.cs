using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.DataObjects.Accounts;

namespace Wams.DataObjects.Admin
{
    public class AdminUserInfo : BaseUserInfo
    {
        public string Password { get; set; }
    }
}
