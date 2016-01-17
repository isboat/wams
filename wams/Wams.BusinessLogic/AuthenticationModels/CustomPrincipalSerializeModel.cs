using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.BusinessLogic.AuthenticationModels
{
    public class CustomPrincipalSerializeModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserLoginRole { get; set; }
        public string MembershipType { get; set; }
    }
}
