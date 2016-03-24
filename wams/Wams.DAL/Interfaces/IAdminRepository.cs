using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.DataObjects.Accounts;
using Wams.DataObjects.Admin;

namespace Wams.DAL.Interfaces
{
    public interface IAdminRepository
    {
        BaseUserInfo Login(string email, string password);

        int CreateAdmin(CreateAdmin admin);
        List<BaseUserInfo> GetAllAdmins();
        AdminUserInfo GetAdmins(int id);
        int EditAdmin(EditAdmin editAdmin);
        int DeleteAdmin(int id);
    }
}
