using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.Interfaces;
using Wams.ViewModels;
using Wams.ViewModels.Account;
using Wams.ViewModels.MemberDues;
using Wams.ViewModels.Registration;

namespace Wams.BusinessLogic
{
    using Wams.DataObjects.Accounts;
    using Wams.DAL.Interfaces;
    using Wams.Enums.Registration;
    using Wams.DataObjects;
    using Wams.ViewModels.MemberInvmt;

    public class AccountLogic : IAccount
    {
        #region Instances variables
        private readonly IAccountRepository accountRepository;

        #endregion

        #region constructors
        public AccountLogic(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }
        #endregion

        public CreateAccountResponse CreateAccount(CreateAccountRequest application)
        {
            var response = new CreateAccountResponse { Status = RegistrationStatus.Failed };

            var memberId = this.accountRepository.CreateApplication(
                application.FirstName,
                application.LastName,
                application.Gender,
                application.DateOfBirth,
                application.EmailAddress,
                application.Address,
                application.Occupation,
                application.Password,
                application.MembershipType,
                application.UserLoginRole);

            if (memberId > 0)
            {
                response.MemberId = memberId;
                response.Status = RegistrationStatus.Accepted;
            }

            return response;
        }

        public Profile GetMemberProfile(int memberId)
        {
            var userProfile = this.accountRepository.GetAccountInfo(memberId);
            return userProfile != null
                ? new Profile
                  {
                      DateOfBirth = userProfile.DateOfBirth,
                      EmailAddress = userProfile.EmailAddress,
                      FirstName = userProfile.FirstName,
                      LastName = userProfile.LastName,
                      Gender = userProfile.Gender,
                      MemberId = userProfile.AccountId,
                      Telephone = userProfile.Telephone,
                      Biography = userProfile.Biography,
                      EmergencyTel = userProfile.EmergencyTel,
                      UserLoginRole = userProfile.LoginRole,
                      MembershipType = userProfile.MembershipType,
                      ProfilePicUrl = userProfile.ProfilePicUrl,
                      CanInvest = userProfile.CanInvest
                  }
                : null;
        }

        public int UpdateProfile(Profile profile)
        {
            if (profile == null)
            {
                return -1;
            }
            var response =
                this.accountRepository.UpdateAccountInfo(
                    new UserAccount
                    {
                        AccountId = profile.MemberId,
                        DateOfBirth = profile.DateOfBirth,
                        EmailAddress = profile.EmailAddress,
                        FirstName = profile.FirstName,
                        Gender = profile.Gender,
                        LastName = profile.LastName,
                        Telephone = profile.Telephone,
                        Biography = profile.Biography,
                        EmergencyTel = profile.EmergencyTel,
                        MembershipType = profile.MembershipType,
                        LoginRole = profile.UserLoginRole,
                        CanInvest = profile.CanInvest
                    });

            return response;
        }

        public int UpdateProfilePicUrl(int accountId, string url)
        {
            return this.accountRepository.UpdateProfilePicUrl(accountId, url);
        }

        public List<Profile> GetUserProfiles()
        {
            var users = this.accountRepository.GetAllUserAccounts();
            if (users != null && users.Count > 0)
            {
                var profiles = new List<Profile>();
                foreach (var user in users)
                {
                    profiles.Add(new Profile
                        {
                            MemberId = user.AccountId,
                            EmailAddress = user.EmailAddress,
                            DateOfBirth = user.DateOfBirth,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            MembershipType = user.MembershipType,
                            Telephone = user.Telephone
                        });
                }

                return profiles;
            }

            return null;
        }

        public BaseResponse DeleteMember(int id)
        {
            var baseResponse = new BaseResponse();
            try
            {
                var rows = this.accountRepository.DeleteMember(id);

                baseResponse.Success = rows == 1;

                return baseResponse;

            }
            catch (Exception exception)
            {
                //log exception.Message here
                return baseResponse;
            }
        }

        #region Member's dues

        public List<MemberDuesViewModel> ViewAllMemberDues(int accountId)
        {
            try
            {
                var list = this.accountRepository.ViewAllMemberDues(accountId);

                var uptodateMonths = this.UptoDateMonths();

                var response = new List<MemberDuesViewModel>();

                foreach (var month in uptodateMonths)
                {
                    var vm = new MemberDuesViewModel { DuesMonth = month.Month, DuesYear = month.Year.ToString() };

                    list.ForEach(x => {

                        if (x.DuesMonth == month.Month && x.DuesYear == month.Year)
                        {
                            vm.Paid = true;
                            vm.DuesId = x.DuesId;
                            vm.MemberId = x.MemberId;
                            vm.MemberName = x.MemberName;
                            vm.DuesYear = x.DuesYear.ToString();
                            vm.Amount = x.Amount;
                            vm.AddedBy = x.AddedBy;
                            vm.AddedDate = x.AddedDate;
                            vm.AddedById = x.AddedById;
                        }
                    });

                    response.Add(vm);
                }

                return response;
            }
            catch (Exception ex)
            {
                //log ex.Message;
                return null;
            }
        }

        private List<MonthViewModel> UptoDateMonths()
        {
            var months = new List<MonthViewModel>();

            var cur = DateTime.Now;

            for (int i = 1; i <= cur.Month; i++)
            {
                if (i <= cur.Month)
                {
                    months.Add(new MonthViewModel 
                        { 
                            Year = cur.Year, 
                            Month = new DateTime(cur.Year, i, 1).ToString("MMM") 
                        });
                }
            }

            return months;
        }

        public BaseResponse AddMemberDues(AddMemberDuesRequest request)
        {
            var baseResponse = new BaseResponse();
            try
            {
                var rows = this.accountRepository.AddMemberDues(new MemberDues
                {
                    MemberId = request.MemberId,
                    MemberName = request.MemberFullName,
                    Amount = request.Amount,
                    DuesMonth = request.DueMonth,
                    DuesYear = request.DueYear,
                    AddedDate = request.AddedDate,
                    AddedBy = request.AddedBy,
                    AddedById = request.AddedById,
                });

                baseResponse.Success = rows == 1;

                return baseResponse;

            }
            catch (Exception exception)
            {
                //log exception.Message here
                return baseResponse;
            }
        }

        public MemberDuesViewModel GetMemberDues(int duesid)
        {
            try
            {
                var dues = this.accountRepository.GetMemberDues(duesid);

                return dues == null ?
                    null :
                    new MemberDuesViewModel
                            {
                                DuesId = dues.DuesId,
                                MemberId = dues.MemberId,
                                MemberName = dues.MemberName,
                                DuesMonth = dues.DuesMonth,
                                DuesYear = dues.DuesYear.ToString(),
                                Amount = dues.Amount,
                                AddedBy = dues.AddedBy,
                                AddedDate = dues.AddedDate,
                                AddedById = dues.AddedById
                            };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public BaseResponse UpdateMemberDues(EditMemberDuesRequest request)
        {
            var baseResponse = new BaseResponse();
            try
            {
                var rows = this.accountRepository.UpdateMemberDues(new MemberDues
                {
                    DuesId = request.DuesId,
                    MemberId = request.MemberId,
                    MemberName = request.MemberFullName,
                    Amount = request.Amount,
                    DuesMonth = request.DueMonth,
                    DuesYear = request.DueYear,
                    AddedDate = request.AddedDate,
                    AddedBy = request.AddedBy,
                    AddedById = request.AddedById,
                });

                baseResponse.Success = rows == 1;

                return baseResponse;

            }
            catch (Exception exception)
            {
                //log exception.Message here
                return baseResponse;
            }
        }

        #endregion

        #region Member's investment

        public List<MemberInvmtViewModel> ViewAllMemberInvestments(int accountId)
        {
            try
            {
                var list = this.accountRepository.ViewAllMemberInvestments(accountId);

                var uptodateMonths = this.UptoDateMonths();

                var response = new List<MemberInvmtViewModel>();

                foreach (var month in uptodateMonths)
                {
                    var vm = new MemberInvmtViewModel { DuesMonth = month.Month, DuesYear = month.Year.ToString() };

                    list.ForEach(x =>
                    {

                        if (x.DuesMonth == month.Month && x.DuesYear == month.Year)
                        {
                            vm.Paid = true;
                            vm.InvmtId = x.InvmtId;
                            vm.MemberId = x.MemberId;
                            vm.MemberName = x.MemberName;
                            vm.DuesYear = x.DuesYear.ToString();
                            vm.Amount = x.Amount;
                            vm.AddedBy = x.AddedBy;
                            vm.AddedDate = x.AddedDate;
                            vm.AddedById = x.AddedById;
                        }
                    });

                    response.Add(vm);
                }

                return response;
            }
            catch (Exception ex)
            {
                //log ex.Message;
                return null;
            }
        }

        public BaseResponse AddMemberInvmt(AddMemberInvmtRequest request)
        {
            var baseResponse = new BaseResponse();
            try
            {
                var rows = this.accountRepository.AddMemberInvmt(new MemberInvmt
                {
                    MemberId = request.MemberId,
                    MemberName = request.MemberFullName,
                    Amount = request.Amount,
                    DuesMonth = request.InvmtMonth,
                    DuesYear = request.InvmtYear,
                    AddedDate = request.AddedDate,
                    AddedBy = request.AddedBy,
                    AddedById = request.AddedById,
                });

                baseResponse.Success = rows == 1;

                return baseResponse;

            }
            catch (Exception exception)
            {
                //log exception.Message here
                return baseResponse;
            }
        }

        public MemberInvmtViewModel GetMemberInvmt(int invmtid)
        {
            try
            {
                var investment = this.accountRepository.GetMemberInvmt(invmtid);

                return investment == null ?
                    null :
                    new MemberInvmtViewModel
                    {
                        InvmtId = investment.InvmtId,
                        MemberId = investment.MemberId,
                        MemberName = investment.MemberName,
                        DuesMonth = investment.DuesMonth,
                        DuesYear = investment.DuesYear.ToString(),
                        Amount = investment.Amount,
                        AddedBy = investment.AddedBy,
                        AddedDate = investment.AddedDate,
                        AddedById = investment.AddedById
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public BaseResponse UpdateMemberInvmt(EditMemberInvmtRequest request)
        {
            var baseResponse = new BaseResponse();
            try
            {
                var rows = this.accountRepository.UpdateMemberInvmt(new MemberInvmt
                {
                    InvmtId = request.InvmtId,
                    MemberId = request.MemberId,
                    MemberName = request.MemberFullName,
                    Amount = request.Amount,
                    DuesMonth = request.InvmtMonth,
                    DuesYear = request.InvmtYear,
                    AddedDate = request.AddedDate,
                    AddedBy = request.AddedBy,
                    AddedById = request.AddedById,
                });

                baseResponse.Success = rows == 1;

                return baseResponse;

            }
            catch (Exception exception)
            {
                //log exception.Message here
                return baseResponse;
            }
        }

        public BaseResponse RequestInvestmentWithdrawal(WithdrawInvestmentRequest request)
        {
            var baseResponse = new BaseResponse();

            try
            {
                var rows = this.accountRepository.RequestInvestmentWithdrawal(new PendingBase
                {
                    Amount = request.Amount,
                    MemberId = request.MemberId,
                    MemberName = request.MemberName,
                    RequestDate = request.RequestDate
                });

                baseResponse.Success = rows == 1;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return baseResponse;
            }
        }

        public WithdrawInvestmentRequest GetInvestmentWithdrawRequest(int id)
        {
            try
            {
                var pending = this.accountRepository.GetInvestmentWithdrawRequest(id);
                if (pending == null)
                {
                    return null;
                }

                return new WithdrawInvestmentRequest
                {
                    MemberId = pending.MemberId,
                    MemberName = pending.MemberName,
                    WithdrawInvmtReqId = pending.PendingId,
                    Amount = pending.Amount,
                    RequestDate = pending.RequestDate,
                    Granted = pending.Granted
                };

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<WithdrawInvestmentRequest> GetAllInvestmentRequests()
        {
            try
            {
                var pendingLoans = this.accountRepository.GetAllInvestmentRequests();
                if (pendingLoans == null)
                {
                    return null;
                }

                return pendingLoans.Select(x =>
                    new WithdrawInvestmentRequest
                    {
                        Amount = x.Amount,
                        MemberId = x.MemberId,
                        MemberName = x.MemberName,
                        RequestDate = x.RequestDate,
                        WithdrawInvmtReqId = x.PendingId
                    }).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public BaseResponse UpdateInvestmentRequest(WithdrawInvestmentRequest request)
        {
            var baseResponse = new BaseResponse();
            try
            {
                var rows = this.accountRepository.UpdateInvestmentRequest(new PendingBase
                {
                    PendingId = request.WithdrawInvmtReqId,
                    MemberId = request.MemberId,
                    MemberName = request.MemberName,
                    RequestDate = request.RequestDate,
                    Amount = request.Amount,
                    Granted = request.Granted
                });

                baseResponse.Success = rows == 1;

                return baseResponse;

            }
            catch (Exception exception)
            {
                //log exception.Message here
                return baseResponse;
            }
        }

        #endregion

        #region Request Loan

        public BaseResponse RequestLoan(LoanRequest request)
        {
            var baseResponse = new BaseResponse();

            try
            {
                var rows = this.accountRepository.RequestLoan(new PendingLoan 
                { 
                    Amount = request.Amount,
                    MemberId = request.MemberId,
                    MemberName = request.MemberName,
                    Reason = request.Reason
                });

                baseResponse.Success = rows == 1;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return baseResponse;
            }
        }

        public List<LoanRequest> GetAllRequestedLoans()
        {
            try
            {
                var pendingLoans = this.accountRepository.GetAllPendingdLoans();
                if (pendingLoans == null)
                {
                    return null;
                }

                return pendingLoans.Select(x =>
                    new LoanRequest
                    {
                        Amount = x.Amount,
                        MemberId = x.MemberId,
                        MemberName = x.MemberName,
                        Reason = x.Reason,
                        PendingLoanId = x.PendingId
                    }).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public LoanRequest GetLoan(int loanid)
        {
            try
            {
                var pending = this.accountRepository.GetPendingdLoan(loanid);
                if (pending == null)
                {
                    return null;
                }

                return new LoanRequest
                {
                    MemberId = pending.MemberId,
                    MemberName = pending.MemberName,
                    Reason = pending.Reason,
                    PendingLoanId = pending.PendingId,
                    Amount = pending.Amount,
                    Granted = pending.Granted
                };

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public BaseResponse UpdateLoan(LoanRequest request)
        {
            var baseResponse = new BaseResponse();
            try
            {
                var rows = this.accountRepository.UpdateLoan(new PendingLoan
                {
                    PendingId = request.PendingLoanId,
                    MemberId = request.MemberId,
                    MemberName = request.MemberName,
                    Reason = request.Reason,
                    Amount = request.Amount,
                    Granted = request.Granted
                });

                baseResponse.Success = rows == 1;

                return baseResponse;

            }
            catch (Exception exception)
            {
                //log exception.Message here
                return baseResponse;
            }
        }

        #endregion

        #region Benefit Request

        public BaseResponse BenefitRequest(BenefitRequest request)
        {
            var baseResponse = new BaseResponse();

            try
            {
                var rows = this.accountRepository.BenefitRequest(new PendingBenefitRequest
                {
                    MemberId = request.MemberId,
                    MemberName = request.MemberName,
                    Message = request.Message,
                    RequestDate = request.RequestDate,
                    BenefitType = request.BenefitType
                });

                baseResponse.Success = rows == 1;

                return baseResponse;
            }
            catch (Exception ex)
            {
                return baseResponse;
            }
        }

        public List<BenefitRequest> GetAllRequestedBenefits()
        {
            try
            {
                var pendingBenefits = this.accountRepository.GetAllPendingdBenefits();
                if (pendingBenefits == null)
                {
                    return null;
                }

                return pendingBenefits.Select(x =>
                    new BenefitRequest
                    {
                        MemberId = x.MemberId,
                        MemberName = x.MemberName,
                        Message = x.Message,
                        BenefitId = x.PendingId,
                        RequestDate = x.RequestDate,
                        BenefitType = x.BenefitType
                    }).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public BenefitRequest GetBenefit(int id)
        {
            try
            {
                var pending = this.accountRepository.GetPendingdBenefits(id);
                if (pending == null)
                {
                    return null;
                }

                return new BenefitRequest
                    {
                        MemberId = pending.MemberId,
                        MemberName = pending.MemberName,
                        Message = pending.Message,
                        BenefitId = pending.PendingId,
                        RequestDate = pending.RequestDate,
                        BenefitType = pending.BenefitType,
                        Granted = pending.Granted
                    };

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public BaseResponse UpdateBenefit(BenefitRequest request)
        {
            var baseResponse = new BaseResponse();
            try
            {
                var rows = this.accountRepository.UpdateBenefit(new PendingBenefitRequest
                {
                    PendingId = request.BenefitId,
                    MemberId = request.MemberId,
                    MemberName = request.MemberName,
                    Message = request.Message,
                    RequestDate = request.RequestDate,
                    BenefitType = request.BenefitType,
                    Granted = request.Granted
                });

                baseResponse.Success = rows == 1;

                return baseResponse;

            }
            catch (Exception exception)
            {
                //log exception.Message here
                return baseResponse;
            }
        }

        #endregion
    }
}
