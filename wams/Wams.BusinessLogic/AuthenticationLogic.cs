using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.DataObjects;
using Wams.Enums.Authentication;
using Wams.Interfaces;
using Wams.ViewModels.Authentication;

namespace Wams.BusinessLogic
{
    using System.Security;
    using System.Web;
    using System.Web.Script.Serialization;
    using System.Web.Security;

    using Wams.BusinessLogic.AuthenticationModels;
    using Wams.Caching;
    using Wams.Common.Configuration;
    using Wams.Common.Helpers;
    using Wams.DAL.Interfaces;

    public class AuthenticationLogic : IAuthentication
    {
        private readonly UpaCarConfiguration upaCarConfiguration = new UpaCarConfiguration();

        private readonly IAccountRepository accountRepository;

        public AuthenticationLogic(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public LoginResponse Login(string username, string password)
        {
            var cacheKey = GlobalCachingProvider.Instance.GetCacheKey("AuthenticationLogic", "Login", username);

            if (GlobalCachingProvider.Instance.ItemExist(cacheKey))
            {
                return (LoginResponse)GlobalCachingProvider.Instance.GetItem(cacheKey);
            }

            var userAccount = this.accountRepository.Login(username, password);
            
            if (userAccount != null)
            {
                var serializeModel = new CustomPrincipalSerializeModel();
                serializeModel.Id = userAccount.AccountId;
                serializeModel.FirstName = userAccount.FirstName;
                serializeModel.LastName = userAccount.LastName;
                serializeModel.Email = userAccount.EmailAddress;
                serializeModel.UserLoginRole = userAccount.UserLoginRole;
                serializeModel.MembershipType = userAccount.MembershipType;

                var response = new LoginResponse();

                if (userAccount.UserLoginRole == 0)
                {
                    response.AuthenticationStatus = AuthenticationStatus.Failed;
                    response.Message = "User login role is 0.";
                    return response;
                }

                var serializer = new JavaScriptSerializer();
                var userData = serializer.Serialize(serializeModel);

                var authTicket = new FormsAuthenticationTicket(
                    1,
                    username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(15),
                    false,
                    userData);
                
                response = new LoginResponse
                           {
                               AuthenticationStatus = AuthenticationStatus.Successful,
                               AccountKey = userAccount.AccountId.ToString(),
                               FirstName = userAccount.FirstName,
                               LastName = userAccount.LastName,
                               EmailAddress = userAccount.EmailAddress,
                               FormsAuthCookieName = FormsAuthentication.FormsCookieName,
                               FormsAuthCookieValue = FormsAuthentication.Encrypt(authTicket)
                           };

                GlobalCachingProvider.Instance.AddItem(cacheKey, response);

                return response;
            }

            return null;
        }

        public ChangePasswordResponse ChangePassword(ChangePasswordRequest request)
        {
            var oldpwd = this.accountRepository.GetPassword(request.AccountKey);
            var response = new ChangePasswordResponse { Status = AuthenticationStatus.Failed };

            if (oldpwd != request.OldPassword)
            {
                return response;
            }

            var res = this.accountRepository.ChangePassword(request.AccountKey, request.NewPassword);
            if (res > 0)
            {
                response.Status = AuthenticationStatus.Successful;
            }

            return response;
        }


        public AuthenticationResponse SetPassCode(string accountKey, string passcodeValue)
        {
            var i = this.accountRepository.SetPasscode(accountKey, passcodeValue);

            var response = new AuthenticationResponse
            {
                AccountKey = accountKey.ToString(),
                AuthenticationStatus = AuthenticationStatus.Failed
            };
            if (i == true)
            {
                response.AuthenticationStatus = AuthenticationStatus.Successful;
            }

            return response;
        }

        public bool ValidateLogon(string accountKey, string lastLogonToken)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetSecurityQuestions()
        {
            throw new NotImplementedException();
        }

        public string CreateSecureToken(DateTime expiresOn, string tokenData)
        {
            throw new NotImplementedException();
        }

        public string GetSecureToken(string token)
        {
            throw new NotImplementedException();
        }

        public string GetUsernameInfo(string token)
        {
            throw new NotImplementedException();
        }

    }
}
