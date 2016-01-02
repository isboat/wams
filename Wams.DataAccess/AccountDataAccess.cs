using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.DataObjects;
using Wams.Interfaces.DAL;

namespace Wams.DataAccess
{
    public class AccountDataAccess : IAccountDataAccess
    {
        public UserAccount Login(string email, string password)
        {
            return new UserAccount { 
                AccountId = 1, 
                DateOfBirth = DateTime.Now, 
                EmailAddress = "em@em.com", 
                FirstName = "First name",
                Gender = "Male",
                LastName = "Lastname"
            };
        }
    }
}
