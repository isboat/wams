using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.ViewModels;

namespace Wams.Interfaces
{
    public interface IAuthentication
    {
        LoginResponse Login(LoginModel model);
    }
}
