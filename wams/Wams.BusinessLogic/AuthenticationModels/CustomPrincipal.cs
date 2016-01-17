using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.BusinessLogic.AuthenticationModels
{
    using System.Security.Principal;

    using Wams.Interfaces;

    public class CustomPrincipal : ICustomPrincipal
    {
        public CustomPrincipal(string email)
        {
            this.Identity = new GenericIdentity(email);
        }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role) { return false; }



        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int UserLoginRole { get; set; }

        public string MembershipType { get; set; }
    }
}
