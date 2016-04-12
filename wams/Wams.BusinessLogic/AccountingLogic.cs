using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.DataObjects.Accounts;
using Wams.DAL.Interfaces;
using Wams.Interfaces;
using Wams.ViewModels.Accounting;

namespace Wams.BusinessLogic
{
    public class AccountingLogic : IAccounting
    {
        
        #region Instances variables
        
        private readonly IAccountRepository accountRepository;

        private readonly IAccountingRepository accountingRepository;

        #endregion

        #region constructors
        
        public AccountingLogic(IAccountRepository accountRepository, IAccountingRepository accountingRepository)
        {
            this.accountRepository = accountRepository;
            this.accountingRepository = accountingRepository;
        }

        #endregion
        public TotalMonthlyDues TotalMonthlyDues(int year)
        {
            var annualDues = this.accountingRepository.GetAll_AnnualDues(year);
            var allMembers = this.accountRepository.GetAllUserAccounts();
            var months = GetMonths(year);

            var total = new TotalMonthlyDues
            {
                AnnualDues = AnnualDues(annualDues, year),
                TotalDuesAmount = annualDues.Sum(x => x.Amount),
                UsersWithFullDues = allMembers.FindAll(x => months.TrueForAll(m => annualDues.Exists(d => d.DuesMonth == m && x.AccountId == d.MemberId))).Count,
                UsersWithNoDues = allMembers.FindAll(x => !annualDues.Exists(m => m.MemberId == x.AccountId)).Count,
                AnnualMonthlyPaidUser = AnnualMonthlyPaidUser(annualDues, year)
            };



            return total;
        }

        private List<KeyValuePair<string, decimal>> AnnualMonthlyPaidUser(List<MemberDues> dues, int year)
        {
            var months = new List<KeyValuePair<string, decimal>>();
            var yearMonths = GetMonths(year);
            
            foreach (var mon in yearMonths)
            {
                var count = dues.FindAll(due => due.DuesYear == year && due.DuesMonth == mon).Count;
                months.Add(new KeyValuePair<string, decimal>(mon, count));
            }

            return months;

        }

        private List<KeyValuePair<string, decimal>> AnnualDues(List<MemberDues> dues, int year)
        {
            var months = new List<KeyValuePair<string, decimal>>();

            for (var i = 1; i <= 12; i++)
            {
                var month = new DateTime(year, i, 1).ToString("MMM");

                var dateAmount = dues.Where(due => due.DuesYear == year && due.DuesMonth == month).Sum(due => due.Amount);
                months.Add(new KeyValuePair<string, decimal>(month, dateAmount));
            }

            return months;
        }

        private List<string> GetMonths(int year)
        {
            var months = new List<string>();

            if (year > DateTime.Now.Year)
            {
                return months;
            }

            var isCurrentYear = DateTime.Now.Year == year;

            var curMonth = DateTime.Now.Month;
            for (var i = 1; i <= (isCurrentYear ? curMonth : 12); i++)
            {
                var d = new DateTime(year, i, 1);
                months.Add(d.ToString("MMM"));
            }

            return months;
        }
    }
}
