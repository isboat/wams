using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wams.Web.Controllers
{
    using Wams.BusinessLogic.AuthenticationModels;

    public class BaseController : Controller
    {
        protected virtual new CustomPrincipal User
        {
            get
            {
                return this.HttpContext.User as CustomPrincipal;
            }
        }


        public ActionResult AboutUs()
        {
           
            return View();
        }

        public ActionResult TermsAndConditions()
        {

            return View();
        }

        public ActionResult CookiePolicy()
        {

            return View();
        }

        public ActionResult PrivacyPolicy()
        {

            return View();
        }

        public ActionResult ContactUs()
        {

            return View();
        }

    }
}