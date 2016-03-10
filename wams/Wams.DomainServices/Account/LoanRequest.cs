using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.ViewModels.Account
{
    public class LoanRequest
    {
        [DisplayName("Loan amount")]
        [Required]
        public decimal Amount { get; set; }

        [DisplayName("Reason for requesting loan")]
        [Required]
        public string Reason { get; set; }

        public int MemberId { get; set; }

        public int PendingLoanId { get; set; }

        [DisplayName("Member's name")]
        public string MemberName { get; set; }

        [DisplayName("Tick to grant")]
        public bool Granted { get; set; }
    }
}
