using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.DataObjects.Accounts;
using Wams.DAL.Interfaces;
using Wams.Interfaces;
using Wams.ViewModels.Accounting;
using Wams.ViewModels.Account;

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
        public TotalData TotalMonthlyDues(int year)
        {
            var annualDues = this.accountingRepository.GetAll_AnnualDues(year);
            var allMembers = this.accountRepository.GetAllUserAccounts();
            var months = GetMonths(year);

            var total = new TotalData
            {
                AnnualChartData = AnnualDues(annualDues, year),
                TotalAmount = annualDues.Sum(x => x.Amount),
                TotalUsersWith = allMembers.FindAll(x => months.TrueForAll(m => annualDues.Exists(d => d.DuesMonth == m && x.AccountId == d.MemberId))).Count,
                TotalUsersWithout = allMembers.FindAll(x => !annualDues.Exists(m => m.MemberId == x.AccountId)).Count,
                AnnualMonthlyPaidUser = AnnualMonthlyPaidUser(annualDues, year)
            };
            
            return total;
        }

        public TotalData InvestmentData(int year)
        {
            var investments = this.accountingRepository.GetAll_Investments(year);
            var allMembers = this.accountRepository.GetAllUserAccounts();

            var total = new TotalData 
            {
                TotalAmount = investments.Sum(x => x.Amount),
                TotalUsersWith =  investments.GroupBy(x => x.MemberId).Select(g => g.First()).Count(),
                TopTenHighestMembers = GetTopTenHighestMembers(investments),
                TotalUsersWithout = allMembers.Where(x => !investments.Exists(e => e.MemberId == x.AccountId)).Count()
            };

            return total;
        }

        private IEnumerable<MemberData> GetTopTenHighestMembers(List<MemberInvmt> investments)
        {
            var users = new List<MemberData>();
            var groups = investments.GroupBy(x => x.MemberId);
            foreach (var g in groups)
            {
                users.Add(new MemberData
                {
                    MemberId = g.First().MemberId,
                    Name = g.First().MemberName,
                    Amount = g.Sum(s => s.Amount)
                });
            }

            return users.OrderByDescending(x => x.Amount).Take(10);
        }

        private Profile GetHighestPaidUser(List<MemberInvmt> investments, out decimal highest)
        {
            highest = 0;
            Profile user = null;
            foreach (var item in investments)
            {
                var temp = investments.Where(x => x.MemberId == item.MemberId).ToList().Sum(x => x.Amount);
                if (temp > highest)
                {
                    highest = temp;
                    user = new Profile
                    {
                       MemberId = item.MemberId,
                       FirstName = item.MemberName
                    };
                }
            }

            return user;
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
