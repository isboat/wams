using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DAL.Interfaces
{
    using Wams.DataObjects;
    using Wams.DataObjects.Accounts;

    public interface IAccountRepository
    {
        BaseUserInfo Login(string email, string password);

        UserAccount GetAccountInfo(int accountid);

        bool SetPasscode(string accountkey, string passcodeKey);

        int CreateApplication(
            string firstname, 
            string lastname, 
            string gender, 
            DateTime dob, 
            string email, 
            string password, 
            string membershipType,
            int userLoginRole);

        int UpdateAccountInfo(UserAccount userAccount);

        int ChangePassword(string accountKey, string newPassword);

        string GetPassword(string accountKey);

        List<UserAccount> GetAllUserAccounts();
    }
}
