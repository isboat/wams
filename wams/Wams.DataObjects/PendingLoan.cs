using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DataObjects
{
    public class PendingLoan
    {
        public int PendingLoanId { get; set; }

        public int MemberId { get; set; }

        public string MemberName { get; set; }

        public decimal Amount { get; set; }

        public string Reason { get; set; }

        public bool Granted { get; set; }
    }
}
