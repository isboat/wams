using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.ViewModels.Accounting
{
    public class TotalMonthlyDues
    {
        public Dictionary<string, decimal> AnnualDues { get; set; }

        public Dictionary<string, int> AnnualMonthlyPaidUser { get; set; }

        public int UsersWithNoDues { get; set; }

        public int UsersWithFullDues { get; set; }

        public decimal TotalDuesAmount { get; set; }
    }
}
