using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DataObjects.Accounts
{
    public class Loan : PendingLoan
    {
        public int LoanId { get; set; }

        public DateTime AddedDate { get; set; }

        public string AddedBy { get; set; }

        public int AddedById { get; set; }
    }
}
