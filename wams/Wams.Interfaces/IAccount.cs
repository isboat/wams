using System;
using System.Collections.Generic;
using System.ServiceModel;
using Wams.ViewModels;
using Wams.ViewModels.Account;
using Wams.ViewModels.MemberDues;
using Wams.ViewModels.MemberInvmt;
using Wams.ViewModels.Registration;

namespace Wams.Interfaces
{
    public interface IAccount
    {
        CreateAccountResponse CreateAccount(CreateAccountRequest application);

        /// <summary>
        /// Gets the user's personal information
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>The user's personal information</returns>
        Profile GetMemberProfile(int memberId);

        int UpdateProfile(Profile profile);

        int UpdateProfilePicUrl(int accountId, string url);

        List<Profile> GetUserProfiles();

        #region Dues

        BaseResponse AddMemberDues(AddMemberDuesRequest request);

        List<MemberDuesViewModel> ViewAllMemberDues(int accountId);

        MemberDuesViewModel GetMemberDues(int duesid);

        BaseResponse UpdateMemberDues(EditMemberDuesRequest model);

        #endregion

        #region Investment

        BaseResponse AddMemberInvmt(AddMemberInvmtRequest request);

        List<MemberInvmtViewModel> ViewAllMemberInvestments(int memberId);

        MemberInvmtViewModel GetMemberInvmt(int invmtid);

        BaseResponse UpdateMemberInvmt(EditMemberInvmtRequest model);

        #endregion

        BaseResponse RequestLoan(LoanRequest request);

        List<LoanRequest> GetAllRequestedLoans();

        BaseResponse BenefitRequest(BenefitRequest request);

        List<BenefitRequest> GetAllRequestedBenefits();

        BenefitRequest GetBenefit(int id);

        BaseResponse UpdateBenefit(BenefitRequest request);
        
        LoanRequest GetLoan(int loanid);
        
        BaseResponse UpdateLoan(LoanRequest request);
    }
}