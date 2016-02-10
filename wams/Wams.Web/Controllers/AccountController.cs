using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wams.ViewModels.Account;
using Wams.ViewModels.Registration;

namespace Wams.Web.Controllers
{
    using System.IO;
    using Wams.Common.IoC;
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
                            Password = req.Password,
                            MembershipType = "None",
                            UserLoginRole = 0
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

        #region Upload Photo
        [HttpGet]
        public ActionResult UploadPhoto()
        {
            if (this.Request.IsAuthenticated)
            {
                return this.View();
            }

            return this.RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        public ActionResult UploadPhoto(HttpPostedFileBase file)
        {
            if (!this.Request.IsAuthenticated)
            {
                return this.RedirectToAction("Login", "Auth");
            }

            //check file was submitted
            if (file != null && file.ContentLength > 0)
            {
                string fname = Guid.NewGuid().ToString("N") +  Path.GetFileName(file.FileName);
                file.SaveAs(Server.MapPath(Path.Combine("~/Images/ProfilePics/", fname)));

                var response = this.accountLogic.UpdateProfilePicUrl(this.User.Id, fname);

            }
            return this.RedirectToAction("Index", "Home");
            //return View();
        }

        #endregion

        #region Dues
        
        public ActionResult ViewMemberDues()
        {
            if (!this.Request.IsAuthenticated)
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var model = this.accountLogic.ViewAllMemberDues(this.User.Id);

            if (model == null)
            {
                return View("BaseResponse",
                    new BaseResponse
                    {
                        Status = BaseResponseStatus.Failed,
                        Message = "Unknown error occured.",
                        HtmlString = new HtmlString("Try again.")
                    });
            }

            return View(model);
        }
        #endregion

        #region Request Loan

        public ActionResult RequestLoan() 
        {
            if (!this.Request.IsAuthenticated)
            {
                return this.RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpPost]
        public ActionResult RequestLoan(LoanRequest request)
        {
            if (!this.Request.IsAuthenticated)
            {
                return this.RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            request.MemberId = this.User.Id;
            request.MemberName = string.Format("{0} {1}", this.User.FirstName, this.User.LastName);

            var model = this.accountLogic.RequestLoan(request);

            if (model == null)
            {
                return this.RedirectToAction("Error");
            }

            return View("BaseResponse",
                !model.Success ?
                    new BaseResponse
                    {
                        Status = BaseResponseStatus.Failed,
                        Message = "Unknown error occured.",
                        HtmlString = new HtmlString("Try again." + model.Message)
                    } :
                    new BaseResponse
                    {
                        Status = BaseResponseStatus.Success,
                        Message = "Your loan is requested successfully. Customer services will contact you soon.",
                        HtmlString = new HtmlString("Update your contact information if they're not update to date.")
                    });
        }

        #endregion

        #region Membership type

        public ActionResult ViewMembershipType()
        {
            if (!this.Request.IsAuthenticated)
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var user = this.accountLogic.GetMemberProfile(this.User.Id);

            if (user == null) return this.RedirectToAction("ViewMyProfile", new { memberId = this.User.Id });

            var model = Tuple.Create<string>(user.MembershipType);
            return View("ViewMembershipType", model);
        }

        #endregion
    }
}