using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DataObjects.Accounts
{
    public class UserAccount
    {
        public int AccountId { get; set; }

        public string Gender { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string EmailAddress { get; set; }

        public string Biography { get; set; }

        public string Telephone { get; set; }

        public string EmergencyTel { get; set; }
    }
}
