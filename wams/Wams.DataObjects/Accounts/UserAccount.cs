using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DataObjects.Accounts
{
    public class UserAccount : BaseUserInfo
    {
        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Biography { get; set; }

        public string Telephone { get; set; }

        public string EmergencyTel { get; set; }

        public string MembershipType { get; set; }
    }

    public class BaseUserInfo
    {
        public int AccountId { get; set; }

        public int UserLoginRole { get; set; }

        public bool CanInvest { get; set; }

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MembershipType { get; set; }

        public string ProfilePicUrl { get; set; }
    }
}
