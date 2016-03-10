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
    using Wams.Common.Logging;
    using Wams.Enums.Registration;
    using Wams.Interfaces;
    using Wams.Web.Models;

    public class AccountController : BaseController
    {
        #region Instances Variables

        private readonly IAccount accountLogic = IoC.Instance.Resolve<IAccount>();
        private readonly ILogProvider logProvider = IoC.Instance.Resolve<ILogProvider>();
        
        #endregion

        #region Register

        // GET: Account
        public ActionResult Register()
        {
            try
            {
                var model = new RegisterRequest
                {
                    MembershipTypeOptions = UIHelper.GetMembershipTypeOptions(),
                    GenderOptions = UIHelper.GetGenderOptions()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                this.logProvider.Error(this.Request.RawUrl, ex);
                throw;
            }
        }

        [HttpPost]
        public ActionResult Register(RegisterRequest req)
        {
            try
            {
                if (req == null)
                {
                    return this.View();
                }

                DateTime dob;
                if (!ModelState.IsValid || !DateTime.TryParse(req.DateOfBirth, out dob))
                {
                    req.MembershipTypeOptions = UIHelper.GetMembershipTypeOptions();
                    req.GenderOptions = UIHelper.GetGenderOptions();
                    return View(req);
                }

                var response =
                    this.accountLogic.CreateAccount(
                        new CreateAccountRequest
                        {
                            FirstName = req.FirstName,
                            LastName = req.LastName,
                            DateOfBirth = dob,
                            EmailAddress = req.EmailAddress,
                            Address = req.Address,
                            Occupation = req.Occupation,
                            Gender = req.Gender,
                            Password = req.Password,
                            MembershipType = req.MembershipType,
                            UserLoginRole = 0
                        });

                var result = new BaseResponse { Status = BaseResponseStatus.Failed};
                if (response.Status == RegistrationStatus.Accepted)
                    result.Status = BaseResponseStatus.Success;

                return this.View("RegisterSuccess", result);
            }
            catch(Exception ex)
            {
                this.logProvider.Error(this.Request.RawUrl, ex);
                return this.View();
            }
        }

        #endregion

        #region My Profile

        [HttpGet]
        public ActionResult ViewMyProfile(int memberId)
        {
            try
            {
                if (this.Request.IsAuthenticated)
                {
                    var profile = this.accountLogic.GetMemberProfile(memberId);

                    return this.View(profile);
                }

                return this.RedirectToAction("Login", "Auth");

            }
            catch (Exception ex)
            {
                this.logProvider.Error(this.Request.RawUrl, ex);
                throw;
            }
        }

        [HttpGet]
        public ActionResult EditMyProfile(int memberId)
        {
            try
            {
                if (this.Request.IsAuthenticated)
                {
                    var profile = this.accountLogic.GetMemberProfile(memberId);
                    return this.View(profile);
                }

                return this.RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                this.logProvider.Error(this.Request.RawUrl, ex);
                throw;
            }
        }

        [HttpPost]
        public ActionResult EditMyProfile(Profile profile)
        {
            try
            {
                if (profile == null || !this.Request.IsAuthenticated)
                {
                    return this.RedirectToAction("Login", "Auth");
                }

                var response = this.accountLogic.UpdateProfile(profile);

                return response > 0 ? this.RedirectToAction("ViewMyProfile", new { memberId = profile.MemberId }) : this.RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                this.logProvider.Error(this.Request.RawUrl, ex);
                throw;
            }
        }

        #endregion

        #region Upload Photo

        [HttpGet]
        public ActionResult UploadPhoto()
        {
            try
            {
                if (this.Request.IsAuthenticated)
                {
                    return this.View();
                }

                return this.RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                this.logProvider.Error(this.Request.RawUrl, ex);
                throw;
            }
        }

        [HttpPost]
        public ActionResult UploadPhoto(HttpPostedFileBase file)
        {
            try
            {
                if (!this.Request.IsAuthenticated)
                {
                    return this.RedirectToAction("Login", "Auth");
                }

                //check file was submitted
                if (file != null && file.ContentLength > 0)
                {
                    string fname = Guid.NewGuid().ToString("N") + Path.GetFileName(file.FileName);
                    file.SaveAs(Server.MapPath(Path.Combine("~/Images/ProfilePics/", fname)));

                    var response = this.accountLogic.UpdateProfilePicUrl(this.User.Id, fname);

                }
                return this.RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                this.logProvider.Error(this.Request.RawUrl, ex);
                throw;
            }
        }

        #endregion

        #region Dues
        
        public ActionResult ViewMemberDues()
        {
            try
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
            catch (Exception ex)
            {
                this.logProvider.Error(this.Request.RawUrl, ex);
                throw;
            }
        }

        public ActionResult ViewMemberOutstanding()
        {
            try
            {
                if (!this.Request.IsAuthenticated)
                {
                    return this.RedirectToAction("Login", "Auth");
                }

                var dues = this.accountLogic.ViewAllMemberDues(this.User.Id);

                if (dues == null)
                {
                    return View("BaseResponse",
                        new BaseResponse
                        {
                            Status = BaseResponseStatus.Failed,
                            Message = "Unknown error occured.",
                            HtmlString = new HtmlString("Try again.")
                        });
                }
                var model = dues.Where(x => !x.Paid).ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                this.logProvider.Error(this.Request.RawUrl, ex);
                throw;
            }
        }

        #endregion

        #region Request Loan

        public ActionResult RequestLoan() 
        {
            try
            {
                if (!this.Request.IsAuthenticated)
                {
                    return this.RedirectToAction("Login", "Auth");
                }

                return View();
            }
            catch (Exception ex)
            {
                this.logProvider.Error(this.Request.RawUrl, ex);
                throw;
            }
        }

        [HttpPost]
        public ActionResult RequestLoan(LoanRequest request)
        {
            try
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
            catch (Exception ex)
            {
                this.logProvider.Error(this.Request.RawUrl, ex);
                throw;
            }
        }

        #endregion

        #region Benefit Request

        public ActionResult BenefitRequest()
        {
            try
            {
                if (!this.Request.IsAuthenticated)
                {
                    return this.RedirectToAction("Login", "Auth");
                }

                var model = new BenefitRequest { BenefitTypeOptions = UIHelper.GetBenefitTypeOptions() };
                return View(model);
            }
            catch (Exception ex)
            {
                this.logProvider.Error(this.Request.RawUrl, ex);
                throw;
            }
            
        }

        [HttpPost]
        public ActionResult BenefitRequest(BenefitRequest request)
        {
            try
            {
                if (!this.Request.IsAuthenticated)
                {
                    return this.RedirectToAction("Login", "Auth");
                }

                if (!ModelState.IsValid)
                {
                    request.BenefitTypeOptions = UIHelper.GetBenefitTypeOptions();
                    return View(request);
                }

                request.MemberId = this.User.Id;
                request.MemberName = string.Format("{0} {1}", this.User.FirstName, this.User.LastName);

                var model = this.accountLogic.BenefitRequest(request);

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
                            Message = "Your benefit is requested successfully. Customer services will contact you soon.",
                            HtmlString = new HtmlString("Update your contact information if they're not update to date.")
                        });
            }
            catch (Exception ex)
            {
                this.logProvider.Error(this.Request.RawUrl, ex);
                throw;
            }
        }

        #endregion

        #region Membership type

        public ActionResult ViewMembershipType()
        {
            try
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
            catch (Exception ex)
            {
                this.logProvider.Error(this.Request.RawUrl, ex);
                throw;
            }
        }

        #endregion
    }
}