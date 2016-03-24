using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.ViewModels;
using Wams.ViewModels.Admin;

namespace Wams.Interfaces
{
    public interface IAdminLogic
    {
        BaseResponse CreateAdmin(RegisterAdminRequest request);
        List<AdminUser> GetAllAdmins();
        AdminUser GetAdmin(int id);
        BaseResponse EditAdmin(EditAdminRequest request);
        BaseResponse DeleteAdmin(int id);
    }
}
