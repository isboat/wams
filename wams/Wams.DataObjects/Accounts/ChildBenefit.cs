using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DataObjects.Accounts
{
    public class ChildBenefit
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public string MemberName { get; set; }

        public decimal Amount { get; set; }

        public string Month { get; set; }

        public int Year { get; set; }

        public string AddedDate { get; set; }

        public string AddedBy { get; set; }

        public int AddedById { get; set; }
    }
}
