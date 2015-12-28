using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.ViewModels
{
    public class LoginResponse : BaseResponse
    {
        public string FormsAuthCookieName { get; set; }

        public string FormsAuthCookieValue { get; set; }
    }
}
