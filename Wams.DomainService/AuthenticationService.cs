using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.Security;
using Wams.Interfaces;
using Wams.Interfaces.DAL;
using Wams.ViewModels;

namespace Wams.DomainService
{
    public class AuthenticationService : IAuthentication
    {
        private readonly IAccountDataAccess accountDataAccess;

        public AuthenticationService(IAccountDataAccess accountDataAccess)
        {
            this.accountDataAccess = accountDataAccess;
        }

        public LoginResponse Login(LoginModel model)
        {
            var userAccount = this.accountDataAccess.Login(model.UserName, model.Password);
            var response = new LoginResponse { Status = Status.Fail };
            if (userAccount != null)
            {
                var serializeModel = new CustomPrincipalSerializeModel();
                serializeModel.Id = userAccount.AccountId;
                serializeModel.FirstName = userAccount.FirstName;
                serializeModel.LastName = userAccount.LastName;
                serializeModel.Email = userAccount.EmailAddress;

                var serializer = new JavaScriptSerializer();
                var userData = serializer.Serialize(serializeModel);

                var authTicket = new FormsAuthenticationTicket(
                    1,
                    model.UserName,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(15),
                    false,
                    userData);

                response = new LoginResponse
                {
                    Status = Status.Success,
                    FormsAuthCookieName = FormsAuthentication.FormsCookieName,
                    FormsAuthCookieValue = FormsAuthentication.Encrypt(authTicket)
                };
            }

            return response;
        }
    }
}
