using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DataObjects
{
    public class PendingBenefitRequest
    {
        public int BenefitId { get; set; }

        public string BenefitType { get; set; }

        public string BenefitDate { get; set; }

        public string Message { get; set; }

        public int MemberId { get; set; }

        public string Address { get; set; }

        public string MemberName { get; set; }

        public bool Granted { get; set; }
    }
}
