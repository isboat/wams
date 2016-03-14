using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.ViewModels.Account
{
    public class LoanRequest : BaseRequest
    {
        [DisplayName("Reason for requesting loan")]
        [Required]
        public string Reason { get; set; }

        public int PendingLoanId { get; set; }
    }
}
