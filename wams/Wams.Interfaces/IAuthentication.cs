using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Wams.DataObjects;
using Wams.ViewModels.Authentication;

namespace Wams.Interfaces
{
    public interface IAuthentication
    {
        LoginResponse Login(string username, string password, bool isAdmin = false);

        ChangePasswordResponse ChangePassword(ChangePasswordRequest request);

        AuthenticationResponse SetPassCode(string accountKey, string passcodeValue);

        bool ValidateLogon(string accountKey, string lastLogonToken);

        IEnumerable<string> GetSecurityQuestions();

        string CreateSecureToken(DateTime expiresOn, string tokenData);

        string GetSecureToken(string token);

        string GetUsernameInfo(string token);
    }
}
