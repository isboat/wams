using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DataObjects
{
    public class PendingBenefitRequest : PendingBase
    {
        public string BenefitType { get; set; }

        public string Message { get; set; }
    }
}
