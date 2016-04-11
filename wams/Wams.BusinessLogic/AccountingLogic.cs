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
            var total = new TotalMonthlyDues
            {
                AnnualDues = AnnualDues(annualDues, year),
                TotalDuesAmount = annualDues.Sum(x => x.Amount)
            };



            return total;
        }

        private Dictionary<string, decimal> AnnualDues(List<MemberDues> dues, int year)
        {
            var months = new Dictionary<string, decimal>();

            for (var i = 1; i <= 12; i++)
            {
                var month = new DateTime(year, i, 1).ToString("MMM");

                var dateAmount = dues.Where(due => due.DuesYear == year && due.DuesMonth == month).Sum(due => due.Amount);
                months.Add(month, dateAmount);
            }

            return months;
        }
    }
}
