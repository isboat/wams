using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.ViewModels.Account;

namespace Wams.ViewModels.Accounting
{
    public class TotalData
    {
        public List<KeyValuePair<string, decimal>> AnnualChartData { get; set; }

        public List<KeyValuePair<string, decimal>> AnnualMonthlyPaidUser { get; set; }

        public int TotalUsersWithout { get; set; }

        public int TotalUsersWith { get; set; }

        public decimal TotalAmount { get; set; }

        public MemberData HighestPaidMember { get; set; }

        public IEnumerable<MemberData> TopTenHighestMembers { get; set; }

        public decimal HighestPaidAmount { get; set; }
    }
}
