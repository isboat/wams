using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wams.ViewModels;

namespace Wams.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var model = new HomepageViewModel();
            
            var qs = this.Request.QueryString["slider"];
            if (!string.IsNullOrEmpty(qs))
            {
                this.Response.Cookies.Add(new HttpCookie("slider", qs));
                model.ShowSlider = qs == "show";
            }
            else
            {
                var cookie = this.Request.Cookies["slider"];
                model.ShowSlider = cookie == null || cookie.Value == "show";
            }

            return this.View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}