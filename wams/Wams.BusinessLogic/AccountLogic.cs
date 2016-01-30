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

                return list == null ? 
                    null :
                    list.Select(
                        x =>
                            new MemberDuesViewModel
                            {
                                DuesId = x.DuesId,
                                MemberId = x.MemberId,
                                MemberName = x.MemberName,
                                DuesMonth = x.DuesMonth,
                                DuesYear = x.DuesYear.ToString(),
                                Amount = x.Amount,
                                AddedBy = x.AddedBy,
                                AddedDate = x.AddedDate.ToShortDateString(),
                                AddedById = x.AddedById
                            }).ToList();
            }
            catch (Exception ex)
            {
                //log ex.Message;
                return null;
            }
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
    }
}
