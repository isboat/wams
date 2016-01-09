﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wams.Web.Controllers
{
    using System.Web.Security;

    using Wams.Common.IoC;
    using Wams.DataObjects;
    using Wams.DomainObjects.Authentication;
    using Wams.Enums.Authentication;
    using Wams.Interfaces;
    using Wams.Web.Models;

    public class AuthController : BaseController
    {

        #region Instances Variables

        private readonly IAuthentication authenticationLogic = IoC.Instance.Resolve<IAuthentication>();

        #endregion
        // GET: Auth
        [HttpGet]
        public ActionResult Login()
        {
            if (this.Request.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return this.View();
                }

                request.WebSource = new Source { Checksum = "dsktpid=12/12/2015", TimeStamp = "12/12/2015"};
                var response = this.authenticationLogic.Login(request.Username, request.Password, request.WebSource);

                if (response != null)
                {

                    var formAuthCookie = new HttpCookie(response.FormsAuthCookieName, response.FormsAuthCookieValue);
                    this.Response.Cookies.Add(formAuthCookie);

                    return this.RedirectToAction("Index", "Home");
                }

                this.ModelState.AddModelError("Username", "username or password is incorrect");
                return this.View();
            }
            catch (Exception ex)
            {
                return this.View();
            }
        }

        public ActionResult LogOff()
        {
            if (!this.Request.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }
            
            var user = this.User;
            
            FormsAuthentication.SignOut();

            return this.RedirectToAction("Index", "Home");
        }

        #region Change password

        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (this.Request.IsAuthenticated)
            {
                return this.View();
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordRequest req)
        {
            var baseResponse = new BaseResponse { Status = BaseResponseStatus.Failed };
            
            if (this.Request.IsAuthenticated)
            {
                req.AccountKey = this.User.Id.ToString();
                var response = this.authenticationLogic.ChangePassword(req);

                if (response.Status == AuthenticationStatus.Successful)
                    baseResponse.Status = BaseResponseStatus.Success;
            }

            return this.View("ChangePasswordResponse", baseResponse);
        }

        #endregion
    }
}