using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.ViewModels.Accounting
{
    public class TotalMonthlyDues
    {
        public List<KeyValuePair<string, decimal>> AnnualDues { get; set; }

        public List<KeyValuePair<string, decimal>> AnnualMonthlyPaidUser { get; set; }

        public int UsersWithNoDues { get; set; }

        public int UsersWithFullDues { get; set; }

        public decimal TotalDuesAmount { get; set; }
    }
}
