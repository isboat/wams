using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wams.Web.Controllers
{
    using Wams.Common.IoC;
    using Wams.DomainObjects.Account;
    using Wams.DomainObjects.Registration;
    using Wams.Enums.Registration;
    using Wams.Interfaces;
    using Wams.Web.Models;

    public class AccountController : BaseController
    {
        #region Instances Variables

        private readonly IAccount accountLogic = IoC.Instance.Resolve<IAccount>();

        #endregion

        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        #region Register

        [HttpPost]
        public ActionResult Register(RegisterRequest req)
        {
            try
            {
                if (req == null)
                {
                    return this.View();
                }

                var response =
                    this.accountLogic.CreateAccount(
                        new CreateAccountRequest
                        {
                            FirstName = req.FirstName,
                            LastName = req.LastName,
                            DateOfBirth = req.DateOfBirth,
                            EmailAddress = req.EmailAddress,
                            Gender = req.Gender,
                            Password = req.Password
                        });

                var result = new BaseResponse { Status = BaseResponseStatus.Failed};
                if (response.Status == RegistrationStatus.Accepted)
                    result.Status = BaseResponseStatus.Success;

                return this.View("RegisterSuccess", result);
            }
            catch
            {
                return this.View();
            }
        }

        #endregion

        #region My Profile

        [HttpGet]
        public ActionResult ViewMyProfile(int memberId)
        {
            if (this.Request.IsAuthenticated)
            {
                var profile = this.accountLogic.GetMemberProfile(memberId);

                return this.View(profile);
            }

            return this.RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult EditMyProfile(int memberId)
        {
            if (this.Request.IsAuthenticated)
            {
                var profile = this.accountLogic.GetMemberProfile(memberId);
                return this.View(profile);
            }

            return this.RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        public ActionResult EditMyProfile(Profile profile)
        {
            if (profile == null || !this.Request.IsAuthenticated)
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var response = this.accountLogic.UpdateProfile(profile);

            return response > 0 ? this.RedirectToAction("ViewMyProfile", new { memberId = profile.MemberId}) : this.RedirectToAction("Index", "Home");
        }

        #endregion

        #region Other profiles

        [HttpGet]
        public ActionResult ViewOtherProfile(int memberId)
        {
            if (memberId < 1)
                return this.RedirectToAction("Index", "Home");

            var model = this.accountLogic.GetMemberProfile(memberId);
            return this.View(model);
        }

        #endregion

    }
}