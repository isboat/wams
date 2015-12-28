using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using Wams.ViewModels;

namespace Wams.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
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
                    DateOfBirth = serializeModel.DateOfBirth,
                    Gender = serializeModel.Gender
                };

                HttpContext.Current.User = newUser;
            }
        }
    }
}