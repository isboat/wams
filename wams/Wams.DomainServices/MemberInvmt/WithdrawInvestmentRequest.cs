using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Wams.ViewModels.MemberInvmt
{
    public class WithdrawInvestmentRequest : BaseRequest
    {
        public int WithdrawInvmtReqId { get; set; }

        [DisplayName("How should we pay you")]
        public string HowToPayYou { get; set; }

        public IEnumerable<SelectListItem> HowToPayYouOptions { get; set; }
    }
}
