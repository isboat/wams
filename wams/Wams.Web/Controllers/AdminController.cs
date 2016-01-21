using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wams.Common.IoC;
using Wams.Interfaces;
using Wams.ViewModels.Account;

namespace Wams.Web.Controllers
{
    public class AdminController : BaseController
    {
        #region Instances Variables

        private readonly IAccount accountLogic = IoC.Instance.Resolve<IAccount>();

        #endregion
        
        //
        public ActionResult Index()
        {
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1)
            {
                return View();
            }

            return this.RedirectToAction("Index", "Home");
        }

        //
        public ActionResult List()
        {
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1)
            {
                var users = this.accountLogic.GetUserProfiles();
                return View(users);
            }

            return this.RedirectToAction("Index", "Home");
        }

        //
        // GET: /Admin/Create
        public ActionResult UserDetails(int id)
        {
            if (!this.Request.IsAuthenticated || this.User.UserLoginRole < 2)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (id == 0)
            {
                return this.RedirectToAction("Index");
            }

            var user = this.accountLogic.GetMemberProfile(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult UserDetails(Profile profile)
        {
            if (!this.Request.IsAuthenticated || this.User.UserLoginRole < 2)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (profile == null)
            {
                return this.RedirectToAction("Index");
            }

            var response = this.accountLogic.UpdateProfile(profile);

            return response > 0 ?
                this.RedirectToAction("List") :
                this.RedirectToAction("UserDetails", new { id = profile.MemberId });
        }

        #region Member's Dues

        //
        public ActionResult AddMemberDues(int id)
        {
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1)
            {
                return View();
            }

            return this.RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
