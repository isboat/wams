using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Wams.Web
{
    using System.Web.Script.Serialization;
    using System.Web.Security;

    using Wams.BusinessLogic.AuthenticationModels;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = this.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var serializer = new JavaScriptSerializer();
                if (authTicket == null)
                {
                    return;
                }
                
                var serializeModel = serializer.Deserialize<CustomPrincipalSerializeModel>(authTicket.UserData);
                var newUser = new CustomPrincipal(authTicket.Name)
                              {
                                  Id = serializeModel.Id,
                                  FirstName = serializeModel.FirstName,
                                  LastName = serializeModel.LastName,
                                  Email = serializeModel.Email,
                                  UserLoginRole = serializeModel.UserLoginRole,
                                  MembershipType = serializeModel.MembershipType
                              };

                HttpContext.Current.User = newUser;
            }
        }
    }
}
