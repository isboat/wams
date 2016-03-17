using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.DataObjects.Accounts;

namespace Wams.DAL.Interfaces
{
    public interface IAdminRepository
    {
        BaseUserInfo Login(string email, string password);
    }
}
