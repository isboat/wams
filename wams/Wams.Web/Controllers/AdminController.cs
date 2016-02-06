﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wams.Common.IoC;
using Wams.Interfaces;
using Wams.ViewModels.Account;
using Wams.ViewModels.MemberDues;
using Wams.Web.Models;

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
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1 && id > 0)
            {
                var member = this.accountLogic.GetMemberProfile(id);
                var model = new AddMemberDuesRequest
                {
                    MemberId = id,
                    MemberFullName = string.Format("{0} {1}", member.FirstName, member.LastName),
                    AddedBy = string.Format("{0} {1}", this.User.FirstName, this.User.LastName),
                    AddedById = this.User.Id,
                    AddedDate = DateTime.Now,
                    DueMonthOptions = UIHelper.GetMonthOptions(),
                    DueYearOptions = UIHelper.GetYearOptions()
                };

                return View(model);
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult AddMemberDues(AddMemberDuesRequest request)
        {
            if (!this.Request.IsAuthenticated || this.User.UserLoginRole < 2)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (request == null)
            {
                return this.RedirectToAction("Error");
            }

            if (!ModelState.IsValid)
            {
                request.DueMonthOptions = UIHelper.GetMonthOptions();
                request.DueYearOptions = UIHelper.GetYearOptions();

                return View(request);
            }

            var response = this.accountLogic.AddMemberDues(request);

            var model = new BaseResponse
            {
                Status = response.Success ? BaseResponseStatus.Success : BaseResponseStatus.Failed,
                Message = response.Message,
                HtmlString = response.Success ?
                    new HtmlString(string.Format("Member's due added. <a href='/Admin/UserDetails/{0}'>Back to member's profile</a>", request.MemberId)) :
                    new HtmlString(string.Format("<a href='/Admin/UserDetails/{0}'>Back to member's profile</a>", request.MemberId))
            };

            return View("BaseResponse", model);
        }

        public ActionResult ViewMemberDues(int id)
        {
            if (!this.Request.IsAuthenticated || this.User.UserLoginRole < 2)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var model = this.accountLogic.ViewAllMemberDues(id);

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

        [HttpGet]
        public ActionResult EditMemberDues(int duesid)
        {
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1)
            {
                var model = this.accountLogic.GetMemberDues(duesid);

                var req = new EditMemberDuesRequest
                {
                    MemberId = model.MemberId,
                    DuesId = model.DuesId,
                    Amount = model.Amount,
                    MemberFullName = model.MemberName,
                    AddedBy = string.Format("{0} {1}", this.User.FirstName, this.User.LastName),
                    AddedById = this.User.Id,
                    AddedDate = DateTime.Now,
                    DueMonth = model.DuesMonth,
                    DueYear = Convert.ToInt32(model.DuesYear),
                    DueMonthOptions = UIHelper.GetMonthOptions(),
                    DueYearOptions = UIHelper.GetYearOptions()
                };

                return View(req);
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult EditMemberDues(EditMemberDuesRequest model)
        {
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1)
            {
                if (model == null)
                {
                    return this.RedirectToAction("Error");
                }

                if (!ModelState.IsValid)
                {
                    model.DueMonthOptions = UIHelper.GetMonthOptions();
                    model.DueYearOptions = UIHelper.GetYearOptions();

                    return View(model);
                }

                var result = this.accountLogic.UpdateMemberDues(model);

                var response = new BaseResponse
                {
                    Status = result.Success ? BaseResponseStatus.Success : BaseResponseStatus.Failed,
                    Message = result.Message,
                    HtmlString = result.Success ?
                        new HtmlString(string.Format("Member's due updated. <a href='/Admin/UserDetails/{0}'>Back to member's profile</a>", model.MemberId)) :
                        new HtmlString(string.Format("<a href='/Admin/UserDetails/{0}'>Back to member's profile</a>", model.MemberId))
                };

                return View("BaseResponse", response);
            }

            return this.RedirectToAction("Index", "Home");
        }

        #endregion

        #region Loan Requests

        public ActionResult ViewLoanRequests()
        {
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1)
            {
                var model = this.accountLogic.GetAllRequestedLoans();
                return View(model);
            }

            return this.RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
