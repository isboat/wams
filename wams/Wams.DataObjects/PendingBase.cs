using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DataObjects
{
    public class PendingBase
    {
        public int PendingId { get; set; }

        public int MemberId { get; set; }

        public string RequestDate { get; set; }

        public string MemberName { get; set; }

        public decimal Amount { get; set; }

        public string Address { get; set; }

        public bool Granted { get; set; }
    }
}
