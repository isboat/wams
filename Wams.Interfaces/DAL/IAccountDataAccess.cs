using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.DataObjects;

namespace Wams.Interfaces.DAL
{
    public interface IAccountDataAccess
    {
        UserAccount Login(string email, string password);
    }
}
