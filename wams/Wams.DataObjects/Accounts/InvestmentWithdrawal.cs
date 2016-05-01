using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DataObjects.Accounts
{
    public class InvestmentWithdrawal : PendingBase
    {
        public string HowToPayYou { get; set; }
    }
}
