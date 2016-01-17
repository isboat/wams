using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DomainObjects.Authentication
{
    using Wams.DataObjects;

    public class LogoutRequest
    {
        public int AccountKey { get; set; }

        public string Username { get; set; }
    }
}
