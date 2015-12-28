using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Wams.ViewModels
{
    public class CustomPrincipal : CustomPrincipalSerializeModel, IPrincipal
    {
        public CustomPrincipal(string email)
        {
            this.Identity = new GenericIdentity(email);
        }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role) { return false; }
    }

    public class CustomPrincipalSerializeModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
