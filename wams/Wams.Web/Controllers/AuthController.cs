using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wams.ViewModels.Authentication;

namespace Wams.Web.Controllers
{
    using System.Web.Security;

    using Wams.Common.IoC;
    using Wams.DataObjects;
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

                var response = this.authenticationLogic.Login(request.Username, request.Password);

                if (response != null && response.AuthenticationStatus == AuthenticationStatus.Successful)
                {
                    var formAuthCookie = new HttpCookie(response.FormsAuthCookieName, response.FormsAuthCookieValue);
                    this.Response.Cookies.Add(formAuthCookie);

                    return this.RedirectToAction("Index", "Home");
                }

                var error = "username or password is incorrect";
                if (response != null && !string.IsNullOrEmpty(response.Message))
                {
                    error = response.Message;
                }

                this.ModelState.AddModelError("Username", error);
                return this.View();
            }
            catch (Exception ex)
            {
                return this.View();
            }
        }

        [HttpGet]
        public ActionResult AdminLogin()
        {
            if (this.Request.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(LoginRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return this.View();
                }

                var response = this.authenticationLogic.Login(request.Username, request.Password, true);

                if (response != null && response.AuthenticationStatus == AuthenticationStatus.Successful)
                {
                    var formAuthCookie = new HttpCookie(response.FormsAuthCookieName, response.FormsAuthCookieValue);
                    this.Response.Cookies.Add(formAuthCookie);

                    return this.RedirectToAction("Index", "Home");
                }

                var error = "username or password is incorrect";
                if (response != null && !string.IsNullOrEmpty(response.Message))
                {
                    error = response.Message;
                }

                this.ModelState.AddModelError("Username", error);
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
