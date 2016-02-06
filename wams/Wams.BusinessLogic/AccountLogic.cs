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
                      UserLoginRole = userProfile.UserLoginRole,
                      MembershipType = userProfile.MembershipType,
                      ProfilePicUrl = userProfile.ProfilePicUrl
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
                        UserLoginRole = profile.UserLoginRole
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
                            vm.AddedDate = x.AddedDate.ToShortDateString();
                            vm.AddedById = x.AddedById;
                        }
                    });

                    response.Add(vm);
                }

                return response;

                //return list == null ? 
                //    null :
                //    list.Select(
                //        x =>
                //            new MemberDuesViewModel
                //            {
                //                DuesId = x.DuesId,
                //                MemberId = x.MemberId,
                //                MemberName = x.MemberName,
                //                DuesMonth = x.DuesMonth,
                //                DuesYear = x.DuesYear.ToString(),
                //                Amount = x.Amount,
                //                AddedBy = x.AddedBy,
                //                AddedDate = x.AddedDate.ToShortDateString(),
                //                AddedById = x.AddedById
                //            }).ToList();
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
                                AddedDate = dues.AddedDate.ToShortDateString(),
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

        #region

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
                        PendingLoanId = x.PendingLoanId
                    }).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
