using System;
using System.Collections.Generic;
using Wams.Enums.Authentication;
using Wams.Interfaces;
using Wams.ViewModels.Authentication;

namespace Wams.BusinessLogic
{
    using System.Web.Script.Serialization;
    using System.Web.Security;

    using AuthenticationModels;
    using Caching;
    using Common.Configuration;
    using DAL.Interfaces;

    public class AuthenticationLogic : IAuthentication
    {
        //private readonly UpaCarConfiguration upaCarConfiguration = new UpaCarConfiguration();

        private readonly IAccountRepository accountRepository;

        private readonly IAdminRepository adminRepository;

        public AuthenticationLogic(IAccountRepository accountRepository, IAdminRepository adminRepository)
        {
            this.accountRepository = accountRepository;
            this.adminRepository = adminRepository;
        }

        public LoginResponse Login(string username, string password, bool isAdmin = false)
        {
            var cacheKey = GlobalCachingProvider.Instance.GetCacheKey("AuthenticationLogic", "Login", username, password, isAdmin.ToString());

            if (GlobalCachingProvider.Instance.ItemExist(cacheKey))
            {
                // return (LoginResponse)GlobalCachingProvider.Instance.GetItem(cacheKey);
            }

            var userAccount = isAdmin ?
                this.adminRepository.Login(username, password) :
                this.accountRepository.Login(username, password);
            
            if (userAccount != null)
            {
                var serializeModel = new CustomPrincipalSerializeModel
                {
                    Id = userAccount.AccountId,
                    FirstName = userAccount.FirstName,
                    LastName = userAccount.LastName,
                    Email = userAccount.EmailAddress,
                    UserLoginRole = userAccount.LoginRole,
                    MembershipType = userAccount.MembershipType,
                    CanInvest = userAccount.CanInvest,
                    CanDoChildBenefit = userAccount.CanDoChildBenefit,
                    IsAdmin = userAccount.IsAdmin
                };

                var response = new LoginResponse();

                if (userAccount.LoginRole == 0)
                {
                    response.AuthenticationStatus = AuthenticationStatus.Failed;
                    response.Message = "Your information has been received, waiting for approval.";
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
    }
}
