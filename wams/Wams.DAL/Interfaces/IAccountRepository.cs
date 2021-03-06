﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DAL.Interfaces
{
    using Wams.DataObjects;
    using Wams.DataObjects.Accounts;

    public interface IAccountRepository
    {
        BaseUserInfo Login(string email, string password);

        UserAccount GetAccountInfo(int accountid);

        bool SetPasscode(string accountkey, string passcodeKey);

        int CreateApplication(
            string firstname, 
            string lastname, 
            string gender, 
            string dob, 
            string email,
            string address,
            string occupation,
            string password, 
            string membershipType,
            int userLoginRole);

        int UpdateAccountInfo(UserAccount userAccount);

        int UpdateProfilePicUrl(int accountId, string url);

        int ChangePassword(string accountKey, string newPassword);

        string GetPassword(string accountKey);

        List<UserAccount> GetAllUserAccounts();

        int DeleteMember(int id);

        int SetMemberPassword(int id, string newPassword);

        #region Dues

        int AddMemberDues(MemberDues dues);

        List<MemberDues> ViewAllMemberDues(int accountId);

        MemberDues GetMemberDues(int duesid);

        int UpdateMemberDues(MemberDues memberDues);

        #endregion

        #region Investment

        int AddMemberInvmt(MemberInvmt investment);

        List<MemberInvmt> ViewAllMemberInvestments(int accountId);

        MemberInvmt GetMemberInvmt(int duesid);

        int UpdateMemberInvmt(MemberInvmt investment);

        int RequestInvestmentWithdrawal(InvestmentWithdrawal withdraw);

        List<InvestmentWithdrawal> GetGrantedMemberInvestmentReqs(int memberId);

        List<PendingBase> GetAllInvestmentRequests();

        InvestmentWithdrawal GetInvestmentWithdrawRequest(int id);

        int UpdateInvestmentRequest(InvestmentWithdrawal pendingBase);

        #endregion

        int RequestLoan(PendingLoan pending);

        List<PendingLoan> GetAllPendingdLoans();

        int BenefitRequest(PendingBenefitRequest pending);

        List<PendingBenefitRequest> GetAllPendingdBenefits();

        PendingBenefitRequest GetPendingdBenefits(int id);

        int UpdateBenefit(PendingBenefitRequest pendingBenefitRequest);
        
        PendingLoan GetPendingdLoan(int loanid);
        
        int UpdateLoan(PendingLoan pendingLoan);

        List<ChildBenefit> ViewAllMemberChildSupport(int id);

        int AddMemberSupport(MemberInvmt memberInvmt);

        MemberInvmt GetMemberSupport(int id);

        int UpdateMemberSupport(MemberInvmt memberInvmt);
    }
}
