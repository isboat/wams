using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DataObjects.Accounts
{
    public class MemberInvmt
    {
        public int InvmtId { get; set; }

        public int MemberId { get; set; }

        public string MemberName { get; set; }

        public decimal Amount { get; set; }

        public string DuesMonth { get; set; }

        public int DuesYear { get; set; }

        public DateTime AddedDate { get; set; }

        public string AddedBy { get; set; }

        public int AddedById { get; set; }
    }
}
