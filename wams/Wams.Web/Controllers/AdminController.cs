using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wams.Common.IoC;
using Wams.Interfaces;
using Wams.ViewModels.Account;
using Wams.ViewModels.Admin;
using Wams.ViewModels.MemberDues;
using Wams.ViewModels.MemberInvmt;
using Wams.Web.Models;

namespace Wams.Web.Controllers
{
    public class AdminController : BaseController
    {
        #region Instances Variables

        private readonly IAccount accountLogic = IoC.Instance.Resolve<IAccount>();

        private readonly IAdminLogic adminLogic = IoC.Instance.Resolve<IAdminLogic>();

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

        public ActionResult DeleteMember(int id)
        {
            if (!this.Request.IsAuthenticated || this.User.UserLoginRole < 2)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (id == 0)
            {
                return this.RedirectToAction("Index");
            }

            var response = this.accountLogic.DeleteMember(id);

            var model = new BaseResponse
            {
                Status = response.Success ? BaseResponseStatus.Success : BaseResponseStatus.Failed,
                Message = response.Message,
                HtmlString = response.Success ?
                    new HtmlString("Member's account is deleted!!") :
                    new HtmlString(string.Format("<a href='/Admin/UserDetails/{0}'>Back to member's profile</a>", id))
            };

            return View("BaseResponse", model);
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
                    AddedDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
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
                    AddedDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
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

        #region Member's Investment

        //
        public ActionResult AddMemberInvmt(int id)
        {
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1 && id > 0)
            {
                var member = this.accountLogic.GetMemberProfile(id);
                var model = new AddMemberInvmtRequest
                {
                    MemberId = id,
                    MemberFullName = string.Format("{0} {1}", member.FirstName, member.LastName),
                    AddedBy = string.Format("{0} {1}", this.User.FirstName, this.User.LastName),
                    AddedById = this.User.Id,
                    InvmtMonthOptions = UIHelper.GetMonthOptions(),
                    InvmtYearOptions = UIHelper.GetYearOptions()
                };

                return View(model);
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult AddMemberInvmt(AddMemberInvmtRequest request)
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
                request.InvmtMonthOptions = UIHelper.GetMonthOptions();
                request.InvmtYearOptions = UIHelper.GetYearOptions();

                return View(request);
            }

            request.AddedDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            var response = this.accountLogic.AddMemberInvmt(request);

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

        public ActionResult ViewMemberInvmt(int id)
        {
            if (!this.Request.IsAuthenticated || this.User.UserLoginRole < 2)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var model = this.accountLogic.ViewAllMemberInvestments(id);

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
        public ActionResult EditMemberInvmt(int invmtid)
        {
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1)
            {
                var model = this.accountLogic.GetMemberInvmt(invmtid);

                var req = new EditMemberInvmtRequest
                {
                    MemberId = model.MemberId,
                    InvmtId = model.InvmtId,
                    Amount = model.Amount,
                    MemberFullName = model.MemberName,
                    AddedBy = string.Format("{0} {1}", this.User.FirstName, this.User.LastName),
                    AddedById = this.User.Id,
                    AddedDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    InvmtMonth = model.DuesMonth,
                    InvmtYear = Convert.ToInt32(model.DuesYear),
                    InvmtMonthOptions = UIHelper.GetMonthOptions(),
                    InvmtYearOptions = UIHelper.GetYearOptions()
                };

                return View(req);
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult EditMemberInvmt(EditMemberInvmtRequest model)
        {
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1)
            {
                if (model == null)
                {
                    return this.RedirectToAction("Error");
                }

                if (!ModelState.IsValid)
                {
                    model.InvmtMonthOptions = UIHelper.GetMonthOptions();
                    model.InvmtYearOptions = UIHelper.GetYearOptions();

                    return View(model);
                }

                var result = this.accountLogic.UpdateMemberInvmt(model);

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

        public ActionResult ViewInvestmentRequests()
        {
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1)
            {
                var model = this.accountLogic.GetAllInvestmentRequests();
                return View(model);
            }

            return this.RedirectToAction("Index", "Home");
        }

        public ActionResult EditInvestmentRequest(int id)
        {
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1)
            {
                var model = this.accountLogic.GetInvestmentWithdrawRequest(id);

                var req = new WithdrawInvestmentRequest
                {
                    MemberId = model.MemberId,
                    WithdrawInvmtReqId = model.WithdrawInvmtReqId,
                    Amount = model.Amount,
                    MemberName = model.MemberName,
                    Granted = model.Granted,
                    RequestDate = model.RequestDate
                };

                return View(req);
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult EditInvestmentRequest(WithdrawInvestmentRequest model)
        {
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1)
            {
                if (model == null)
                {
                    return this.RedirectToAction("Error");
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var result = this.accountLogic.UpdateInvestmentRequest(model);

                var response = new BaseResponse
                {
                    Status = result.Success ? BaseResponseStatus.Success : BaseResponseStatus.Failed,
                    Message = result.Message,
                    HtmlString = result.Success ?
                        new HtmlString(string.Format("Member's investment request is updated. <a href='/Admin/UserDetails/{0}'>Back to member's profile</a>", model.MemberId)) :
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

        [HttpGet]
        public ActionResult EditLoan(int loanid)
        {
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1)
            {
                var model = this.accountLogic.GetLoan(loanid);

                var req = new LoanRequest 
                {
                    MemberId = model.MemberId,
                    PendingLoanId = model.PendingLoanId,
                    Amount = model.Amount,
                    MemberName = model.MemberName,
                    Granted = model.Granted,
                    Reason = model.Reason
                };

                return View(req);
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult EditLoan(LoanRequest model)
        {
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1)
            {
                if (model == null)
                {
                    return this.RedirectToAction("Error");
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var result = this.accountLogic.UpdateLoan(model);

                var response = new BaseResponse
                {
                    Status = result.Success ? BaseResponseStatus.Success : BaseResponseStatus.Failed,
                    Message = result.Message,
                    HtmlString = result.Success ?
                        new HtmlString(string.Format("Member's loan updated. <a href='/Admin/UserDetails/{0}'>Back to member's profile</a>", model.MemberId)) :
                        new HtmlString(string.Format("<a href='/Admin/UserDetails/{0}'>Back to member's profile</a>", model.MemberId))
                };

                return View("BaseResponse", response);
            }

            return this.RedirectToAction("Index", "Home");
        }

        #endregion

        #region Benefits

        public ActionResult ViewBenefitRequests()
        {
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1)
            {
                var model = this.accountLogic.GetAllRequestedBenefits();
                return View(model);
            }

            return this.RedirectToAction("Index", "Home");
        }

        public ActionResult EditBenefit(int id)
        {
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1)
            {
                var model = this.accountLogic.GetBenefit(id);
                return View(model);
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult EditBenefit(BenefitRequest model)
        {
            if (this.Request.IsAuthenticated && this.User.UserLoginRole > 1)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var response = this.accountLogic.UpdateBenefit(model);
                
                return View("BaseResponse",
                    !response.Success ?
                        new BaseResponse
                        {
                            Status = BaseResponseStatus.Failed,
                            Message = "Unknown error occured.",
                            HtmlString = new HtmlString("Try again." + model.Message)
                        } :
                        new BaseResponse
                        {
                            Status = BaseResponseStatus.Success,
                            Message = "You have updated the benefit successfully.",
                            HtmlString = new HtmlString("Contact the member to let them know about this.")
                        });
            }

            return this.RedirectToAction("Index", "Home");
        }

        #endregion

        #region Admin User

        public ActionResult CreateAdmin()
        {
            if (!this.Request.IsAuthenticated || this.User.UserLoginRole < 2)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var request = new RegisterAdminRequest
            {
                RoleOptions = UIHelper.GetPriviledgeOptions()
            };
            return View(request);
        }

        [HttpPost]
        public ActionResult CreateAdmin(RegisterAdminRequest request)
        {
            if (!this.Request.IsAuthenticated || this.User.UserLoginRole < 2)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!this.ModelState.IsValid)
            {
                request.RoleOptions = UIHelper.GetPriviledgeOptions();
                return View(request);
            }

            var response = this.adminLogic.CreateAdmin(request);

            var model = new BaseResponse
            {
                Status = response.Success ? BaseResponseStatus.Success : BaseResponseStatus.Failed,
                Message = response.Message,
                HtmlString = response.Success ?
                    new HtmlString("Admin member is now created!!") :
                    new HtmlString("Error occurred creating admin member")
            };

            return View("BaseResponse", model);
        }

        public ActionResult ViewAdmins()
        {
            if (!this.Request.IsAuthenticated || this.User.UserLoginRole < 2)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var admins = this.adminLogic.GetAllAdmins();

            return View(admins);
        }

        public ActionResult EditAdmin(int id)
        {
            if (!this.Request.IsAuthenticated || this.User.UserLoginRole < 2)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var user = this.adminLogic.GetAdmin(id);

            return View(new EditAdminRequest
            {
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = id,
                Role = user.Role,
                RoleOptions = UIHelper.GetPriviledgeOptions(),
                Password = user.Password,
                ConfirmPassword = user.Password
            });
        }

        [HttpPost]
        public ActionResult EditAdmin(EditAdminRequest request)
        {

            if (!this.Request.IsAuthenticated || this.User.UserLoginRole < 2)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!this.ModelState.IsValid)
            {
                request.RoleOptions = UIHelper.GetPriviledgeOptions();
                return View(request);
            }

            var response = this.adminLogic.EditAdmin(request);

            var model = new BaseResponse
            {
                Status = response.Success ? BaseResponseStatus.Success : BaseResponseStatus.Failed,
                Message = response.Message,
                HtmlString = response.Success ?
                    new HtmlString("Admin member is now created!!") :
                    new HtmlString("Error occurred creating admin member")
            };

            return View("BaseResponse", model);
        }

        public ActionResult DeleteAdmin(int id)
        {
            if (!this.Request.IsAuthenticated || this.User.UserLoginRole < 2)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var response = this.adminLogic.DeleteAdmin(id);
            
            var model = new BaseResponse
            {
                Status = response.Success ? BaseResponseStatus.Success : BaseResponseStatus.Failed,
                Message = response.Message,
                HtmlString = response.Success ?
                    new HtmlString("Admin member is now deleted!!") :
                    new HtmlString("Error occurred creating admin member")
            };

            return View("BaseResponse", model);
        }
        #endregion
    }
}
