using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.DomainObjects.Account;
using Wams.Interfaces;

namespace Wams.BusinessLogic
{
    using Wams.DataObjects.Accounts;
    using Wams.DAL.Interfaces;
    using Wams.DomainObjects.Registration;
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
    }
}
